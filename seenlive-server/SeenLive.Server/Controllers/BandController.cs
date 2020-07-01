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
        private IEnumerable<ConcertEntryDTO> _concertEntries;

        [HttpPost]
        public IActionResult AddConcertEntry(ConcertEntryDTO concertEntry)
        {
            // TODO: save as JSON object to local file for now

            _concertEntries ??= new List<ConcertEntryDTO>();

            _concertEntries.ToList().Add(concertEntry);

            return Ok();
        }

        [HttpGet]
        public ActionResult<IEnumerable<ConcertEntryDTO>> GetConcertEntries()
        {
            return Ok(_concertEntries);
        }
    }
}