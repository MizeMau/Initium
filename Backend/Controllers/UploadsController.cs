using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UploadsController : Controller
    {
        [HttpGet("{**path}")]
        public IActionResult Get(string path)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            var fullPath = Path.Combine(basePath, "uploads", path);

            // Prevent directory traversal attacks
            var uploadsRoot = Path.GetFullPath(Path.Combine(basePath, "uploads"));
            var requestedPath = Path.GetFullPath(fullPath);
            if (!requestedPath.StartsWith(uploadsRoot))
                return BadRequest();

            if (!System.IO.File.Exists(fullPath))
                return NotFound();
            
            var mimeType = GetMimeType(fullPath);
            return PhysicalFile(fullPath, mimeType);
        }

        private string GetMimeType(string path)
        {
            return Path.GetExtension(path).ToLower() switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
