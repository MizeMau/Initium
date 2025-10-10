using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GoogleController : ControllerBase
    {
        [HttpGet("autocomplete")]
        public async Task<object> Search_GET(string query)
        {

            using HttpClient client = new();
            var url = $"https://suggestqueries.google.com/complete/search?hl=en&gl=US&client=chrome&q={query}";
            var response = await client.GetStringAsync(url);

            using JsonDocument doc = JsonDocument.Parse(response);
            var suggestionsArray = doc.RootElement[1];
            string[] suggestions = suggestionsArray.EnumerateArray()
                                               .Select(e => e.GetString()!)
                                               .ToArray();
            return suggestions;
        }
    }
}
