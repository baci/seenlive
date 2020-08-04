using Microsoft.AspNetCore.Mvc;
using SeenLive.Server.Models;
using System.Collections.Generic;
using System.Linq;

namespace SeenLive.Server.Controllers
{  
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private static IList<ArtistEntryDTO> _artistEntries;

        private static int _newGuid = 0;

        [HttpPost]
        public ActionResult<IEnumerable<ArtistEntryDTO>> AddArtistEntry(ArtistEntryDTO artistEntry)
        {
            if (artistEntry == null)
                return BadRequest();

            // TODO: put this into the business logic / CQRS part

            // TODO: save as JSON object to local file for now
            _artistEntries ??= new List<ArtistEntryDTO>();

            // TODO: generate IDs properly, use a proper request DTO for access safety
            if (_artistEntries.ToList().Exists(entry => entry.Artist == artistEntry.Artist))
            {
                artistEntry.DateEntries.FirstOrDefault().Id = (_newGuid++).ToString();
                _artistEntries.Where(entry => entry.Artist == artistEntry.Artist).Single().DateEntries.Add(artistEntry.DateEntries.FirstOrDefault());
            }
            else
            {                
                artistEntry.Id = (_newGuid++).ToString();
                artistEntry.DateEntries.FirstOrDefault().Id = (_newGuid++).ToString();

                _artistEntries.Add(artistEntry);
            }            

            return Ok(_artistEntries);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArtistEntryDTO>> GetArtistEntries()
        {
            return Ok(_artistEntries);
        }
    }
}