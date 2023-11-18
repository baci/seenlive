using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeenLive.Core.Abstractions;

namespace SeenLive.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Authorize(string username, string password)
        {
            var user = _userRepository.Create(string.Empty, username);
            
            // TODO: Implement authentication
            
            return Ok();
        }
    }
}