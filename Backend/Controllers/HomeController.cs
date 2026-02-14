using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            var dbTablelManagementTaskService = new Database.Table.Management.Task.Service();
            //var task = dbTablelManagementTaskService.GetById(10);
            //var success = dbTablelManagementTaskService.UpdateSortNumber(task!, 1);

            var success = dbTablelManagementTaskService.CalculateSortNumbers(1);

            return Ok(success);
        }
    }
}
