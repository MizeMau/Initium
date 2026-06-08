using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : Controller
    {
        [HttpGet("")]
        public IActionResult Pokemon_GET()
        {
            return Ok();
        }
    }
}
