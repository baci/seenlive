﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeenLive.Core.DTOs;
using SeenLive.Web.Handler.Bands;
using SeenLive.Web.Handler.DTOs;

namespace SeenLive.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task <ActionResult<IEnumerable<ArtistResponseDTO>>> AddArtistEntry(ArtistCreationRequestDTO artistRequest)
        {
            if (artistRequest == null)
            {
                return BadRequest("Artist creation request is missing");
            }

            try
            {
                await _mediator.Send(new AddArtistEntryRequest(artistRequest));

                IEnumerable<ArtistResponseDTO> result = await _mediator.Send(new GetArtistEntriesRequest());

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDateEntry(DateEntryDeleteRequestDTO deletionRequest)
        {
            if (deletionRequest == null)
            {
                return BadRequest("Deletion request is missing");
            }

            try
            {
                bool deletedDate = await _mediator.Send(new DeleteDateEntryRequest(){ArtistId = deletionRequest.ArtistId, DateId = deletionRequest.DateId});
                
                return deletedDate
                    ? Ok()
                    : NotFound();
            }
            catch (Exception)
            {
                return BadRequest();
            }            
        }

        [HttpPost]
        public async Task<ActionResult> DeleteArtistEntry(ArtistDeleteRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Deletion request is missing");
            }

            try
            {
                bool deletedArtist = await _mediator.Send(new DeleteArtistEntryRequest { ArtistEntryId = request.ArtistEntryId });
                
                return deletedArtist
                    ? Ok() 
                    : NotFound();
            }
            catch(Exception)
            {
                return BadRequest();
            }            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArtistResponseDTO>>> GetArtistEntries()
        {
            try
            {
                IEnumerable<ArtistResponseDTO> ret = await _mediator.Send(new GetArtistEntriesRequest());
                
                return Ok(ret);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}