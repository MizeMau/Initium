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
        [HttpGet("project/{managementProjectID}")]
        public IActionResult Project_Full_GET(long managementProjectID)
        {
            var userIDString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIDString == null)
                return Unauthorized();

            var dbTableManagementProjectService = new Database.Table.Management.Project.Service();
            var dbTableManagementSectionService = new Database.Table.Management.Section.Service();
            var dbTableManagementTaskService = new Database.Table.Management.Task.Service();

            var dbproject = dbTableManagementProjectService.GetById(managementProjectID);
            if (dbproject == null)
                return NoContent();
            var project = Util.Converter.Convert<Database.Table.Management.Project.DTO.Project>(dbproject);

            var dbSections = dbTableManagementSectionService.GetAllByProject(dbproject);
            project.Sections = Util.Converter.Convert<Database.Table.Management.Section.DTO.Section>(dbSections);
            if (project.Sections.Count == 0) 
                return Ok(project);

            var dbTasks = dbTableManagementTaskService.GetAllByProject(dbproject);
            foreach (var section in project.Sections)
            {
                section.Tasks = dbTasks.Where(t => t.ManagementSectionID == section.ManagementSectionID).ToList();
            }

            return Ok(project);
        }
        [HttpGet("section")]
        public IActionResult Section_GET()
        {
            var dbTablelManagementSectionService = new Database.Table.Management.Section.Service();
            var sections = dbTablelManagementSectionService.GetAll(Request);
            return Ok(sections);
        }
    }
}
