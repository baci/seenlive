using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BandController> _logger;

        public BandController(IMediator mediator, ILogger<BandController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task <ActionResult<IEnumerable<ArtistResponseDTO>>> AddArtistEntry(ArtistCreationRequestDTO artistRequest)
        {
            try
            {
                await _mediator.Send(new AddArtistEntryRequest(artistRequest));

                IEnumerable<ArtistResponseDTO> result = await _mediator.Send(new GetArtistEntriesRequest());

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDateEntry(DateEntryDeleteRequestDTO deletionRequest)
        {
            try
            {
                bool deletedDate = await _mediator.Send(new DeleteDateEntryRequest(){ArtistId = deletionRequest.ArtistId, DateId = deletionRequest.DateId});
                
                return deletedDate
                    ? Ok()
                    : NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Internal Server Error");
            }            
        }

        [HttpPost]
        public async Task<ActionResult> DeleteArtistEntry(ArtistDeleteRequestDTO request)
        {
            try
            {
                bool deletedArtist = await _mediator.Send(new DeleteArtistEntryRequest { ArtistEntryId = request.ArtistEntryId });
                
                return deletedArtist
                    ? Ok() 
                    : NotFound();
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Internal Server Error");
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
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Internal Server Error");
            }
        }
    }
}