using Microsoft.AspNetCore.Mvc;
using SeenLive.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeenLive.Server.Controllers
{  
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private static IList<ArtistEntry> _artistEntries;

        private static int _newArtistGuid = 0;
        private static int _newDateGuid = 0;

        [HttpPost]
        public ActionResult<IEnumerable<ArtistEntry>> AddArtistEntry(ArtistCreationRequestDTO artistRequest)
        {
            if (artistRequest == null)
                return BadRequest("Artist creation request is missing");

            try
            {
                // TODO: put this into the business logic / CQRS part

                // TODO: save as JSON object to local file for now
                _artistEntries ??= new List<ArtistEntry>();

                var artistEntry = _artistEntries.SingleOrDefault(entry => entry.ArtistName == artistRequest.ArtistName);
                if (artistEntry == null)
                {
                    artistEntry = new ArtistEntry("Artist-" + (_newArtistGuid++).ToString(), artistRequest.ArtistName);
                    _artistEntries.Add(artistEntry);
                }
                artistRequest.DateEntryRequests.ToList().ForEach(dateEntry =>
                {
                    artistEntry.DateEntries.Add(new DateEntry()
                    {
                        Id = "Date-" + (_newDateGuid++).ToString(),
                        Date = dateEntry.Date,
                        Location = dateEntry.Location,
                        Remarks = dateEntry.Remarks
                    });
                });

                return Ok(_artistEntries);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArtistEntry>> GetArtistEntries()
        {
            return Ok(_artistEntries);
        }
    }
}