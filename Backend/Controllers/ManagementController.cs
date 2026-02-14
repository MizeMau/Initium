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

            var tmpDBTasks = dbTableManagementTaskService.GetAllByProject(dbproject);
            var dbTasks = Util.Converter.Convert<Database.Table.Management.Task.DTO.Task>(tmpDBTasks);

            var tmpTasks = tmpDBTasks.Where(w => w.ManagementTaskID_Head != null);
            foreach(var tmpTask in tmpTasks)
            {
                var tmpTask_Head = dbTasks.First(f => f.ManagementTaskID == tmpTask.ManagementTaskID_Head);
                tmpTask_Head.Tasks.Add(tmpTask);
                int index = dbTasks.FindIndex(f => f.ManagementTaskID == tmpTask.ManagementTaskID);
                dbTasks.RemoveAt(index);
            }

            var sectionsTasks = dbTasks.Where(w => w.ManagementTaskID_Head == null);
            foreach (var section in project.Sections)
            {
                var sectionTasks = sectionsTasks.Where(t => t.ManagementSectionID == section.ManagementSectionID).ToList();
                section.Tasks = sectionTasks.OrderBy(o => o.SortNumber).ToList();
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
        [HttpPost("section")]
        public IActionResult Section_POST(Database.Table.Management.Section.Model model)
        {
            var dbTablelManagementSectionService = new Database.Table.Management.Section.Service();
            var section = dbTablelManagementSectionService.Create(model);
            return Ok(section);
        }
        [HttpPut("section")]
        public IActionResult Section_PUT(Database.Table.Management.Section.Model model)
        {
            var dbTablelManagementSectionService = new Database.Table.Management.Section.Service();
            var section = dbTablelManagementSectionService.Update(model);
            return Ok(section);
        }
        [HttpDelete("section")]
        public IActionResult Section_DELETE([FromQuery] long id)
        {
            var dbTablelManagementSectionService = new Database.Table.Management.Section.Service();
            var section = dbTablelManagementSectionService.Delete(id);
            return Ok(section);
        }
        [HttpGet("task")]
        public IActionResult Task_GET()
        {
            var dbTablelManagementTaskService = new Database.Table.Management.Task.Service();
            var tasks = dbTablelManagementTaskService.GetAll(Request);
            return Ok(tasks);
        }
        [HttpPost("task")]
        public IActionResult Task_POST(Database.Table.Management.Task.Model model)
        {
            var dbTablelManagementTaskService = new Database.Table.Management.Task.Service();
            var task = dbTablelManagementTaskService.Create(model);
            return Ok(task);
        }
        [HttpPut("task")]
        public IActionResult Task_PUT(Database.Table.Management.Task.Model model)
        {
            var dbTablelManagementTaskService = new Database.Table.Management.Task.Service();
            var task = dbTablelManagementTaskService.Update(model);
            return Ok(task);
        }
        [HttpDelete("task")]
        public IActionResult Task_DELETE([FromQuery] long id)
        {
            var dbTablelManagementTaskService = new Database.Table.Management.Task.Service();
            var success = dbTablelManagementTaskService.Delete(id);
            return Ok(success);
        }
        [HttpPut("task/section")]
        public IActionResult Task_Section_PUT([FromBody] Database.Table.Management.Task.DTO.UpdateTaskSection updateTaskSection)
        {
            var dbTablelManagementTaskService = new Database.Table.Management.Task.Service();
            var success = dbTablelManagementTaskService.UpdateProperty(updateTaskSection.ManagementTaskID, u => u.ManagementSectionID, updateTaskSection.ManagementSectionID);
            return Ok(success);
        }
        [HttpPut("task/sortNumber")]
        public IActionResult Task_Sortnumber_PUT([FromQuery] int index, Database.Table.Management.Task.Model model)
        {
            var dbTablelManagementTaskService = new Database.Table.Management.Task.Service();
            bool success = dbTablelManagementTaskService.UpdateSortNumber(model, index);
            return Ok(success);
        }
    }
}
