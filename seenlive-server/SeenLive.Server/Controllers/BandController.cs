using Microsoft.AspNetCore.Mvc;
using SeenLive.Server.Models;
using System.Collections.Generic;

namespace SeenLive.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BandController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddConcertEntry(ConcertEntryDTO concertEntry)
        {
            // TODO: vorerst Objekt in lokalem File als JSON speichern

            return NotFound();
        }

        [HttpGet]
        public ActionResult<IEnumerable<ConcertEntryDTO>> GetConcertEntries()
        {
            // TODO: vorerst Objekte aus lokalem File laden

            return NotFound();
        }
    }
}