using Microsoft.AspNetCore.Mvc;
using SeenLive.Server.Models;
using SeenLive.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeenLive.Server.Controllers
{  
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly ArtistService _artistService;
        private readonly DatesService _datesService;

        public BandController(ArtistService artistService, DatesService datesService)
        {
            _artistService = artistService;
            _datesService = datesService;
        }

        [HttpPost]
        public ActionResult<IEnumerable<ArtistResponseDTO>> AddArtistEntry(ArtistCreationRequestDTO artistRequest)
        {
            if (artistRequest == null)
                return BadRequest("Artist creation request is missing");

            try
            {
                // TODO: extract this into a business logic / CQRS layer

                ArtistEntry artistEntry = _artistService.Get().SingleOrDefault(entry => entry.ArtistName == artistRequest.ArtistName);
                IEnumerable<string> dateEntryIDs = CreateDateEntries(artistRequest.DateEntryRequests);

                if (artistEntry == null)
                {
                    artistEntry = new ArtistEntry(string.Empty, artistRequest.ArtistName, dateEntryIDs);
                    _artistService.Create(artistEntry);
                }
                else
                {
                    artistEntry.AddDateEntries(dateEntryIDs);
                    _artistService.Update(artistEntry.Id, artistEntry);
                }

                return Ok(GetAndConvertArtistEntries());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArtistResponseDTO>> GetArtistEntries()
        {
            try
            {
                return Ok(GetAndConvertArtistEntries());
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private IEnumerable<string> CreateDateEntries(IEnumerable<DateEntryCreationRequestDTO> requests)
        {
            return requests.Select(request =>
            {
                DateEntry newDateEntry = new DateEntry { Id = string.Empty, Date = request.Date, Location = request.Location, Remarks = request.Remarks };
                newDateEntry = _datesService.Create(newDateEntry);
                return newDateEntry?.Id;
            });
        }

        private IEnumerable<ArtistResponseDTO> GetAndConvertArtistEntries()
        {
            return _artistService.Get().Select(entry =>
                    new ArtistResponseDTO(entry.Id, entry.ArtistName, entry.DateEntryIDs.Select(dateId => _datesService.Get(dateId)).ToList()));

        }
    }
}