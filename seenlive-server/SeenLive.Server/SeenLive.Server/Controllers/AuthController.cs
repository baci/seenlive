using Microsoft.AspNetCore.Mvc;

namespace SeenLive.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Authorize(string username, string password)
        {
            // TODO
            return NotFound();
        }
    }
}