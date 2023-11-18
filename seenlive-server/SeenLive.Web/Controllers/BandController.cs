using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeenLive.Web.Handler.DTOs;
using SeenLive.Web.Handler.Requests;

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
        public async Task<ActionResult<IEnumerable<ArtistResponseDTO>>> AddArtistEntry(ArtistCreationRequestDTO artistRequest, CancellationToken token)
        {
            try
            {
                await _mediator.Send(new AddOrUpdateArtistEntryRequest(artistRequest), token);

                IEnumerable<ArtistResponseDTO> result = await _mediator.Send(new GetArtistEntriesRequest
                {
                    UserId = artistRequest.UserId
                }, token);
                
                _logger.LogDebug($"User {artistRequest.UserId} added artist {artistRequest.ArtistName} with {artistRequest.DateEntryRequests.Count()} date entries");

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDateEntry(DateEntryDeleteRequestDTO deletionRequest, CancellationToken token)
        {
            try
            {
                bool deletedDate = await _mediator.Send(new DeleteDateEntryRequest
                {
                    ArtistId = deletionRequest.ArtistId, 
                    DateId = deletionRequest.DateId,
                    UserId = deletionRequest.UserId
                }, token);
                
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
        public async Task<ActionResult> DeleteArtistEntry(ArtistDeleteRequestDTO request, CancellationToken token)
        {
            try
            {
                bool deletedArtist = await _mediator.Send(new DeleteArtistEntryRequest
                {
                    ArtistEntryId = request.ArtistEntryId,
                    UserId = request.UserId,
                }, token);
                
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
        public async Task<ActionResult<IEnumerable<ArtistResponseDTO>>> GetArtistEntries(string userId, CancellationToken token)
        {
            try
            {
                IEnumerable<ArtistResponseDTO> ret = await _mediator.Send(new GetArtistEntriesRequest
                {
                    UserId = userId
                }, token);
                
                _logger.LogDebug($"GetArtistEntries called for user {userId} with {ret.Count()} artist entries");
                
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