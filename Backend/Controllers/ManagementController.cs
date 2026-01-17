using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ManagementController : Controller
    {
        [HttpGet("project")]
        public IActionResult Project_GET()
        {
            var dbTableManagementProjectService = new Database.Table.Management.Project.Service();

            var userIDString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIDString == null)
                return Unauthorized();

            var userID = long.Parse(userIDString);
            var projects = dbTableManagementProjectService.GetAllWithAccess(userID, Request);

            return Ok(projects);
        }
    }
}
