using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SeenLive.Core.Abstractions;
using SeenLive.Core.Abstractions.Models;
using SeenLive.Core.DTOs;
using SeenLive.Web.Handler.Bands;

namespace SeenLive.Web.Controllers
{  
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly IDatesService _datesService;
        private readonly IMediator _mediator;

        public BandController(IArtistService artistService, IDatesService datesService, IMediator mediator)
        {
            _artistService = artistService;
            _datesService = datesService;
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
                IEnumerable<ArtistResponseDTO> result = await _mediator.Send(new AddArtistEntryRequest { ArtistRequest = artistRequest });
                
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
        public ActionResult<IEnumerable<ArtistResponseDTO>> GetArtistEntries()
        {
            try
            {
                return Ok(_artistService.Get().Adapt<IEnumerable<ArtistResponseDTO>>());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}