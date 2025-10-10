using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        [HttpGet("user/login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Example: validate user (replace with your DB logic)
            if (username == "test" && password == "12")
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("LastChanged", DateTime.UtcNow.ToString())
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // <--- persists across browser restarts
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return Ok(new { message = "Logged in" });
            }

            return Unauthorized();
        }

        [HttpGet("user/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out" });
        }

        [HttpGet("user/currentUser")]
        public IActionResult Me()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return Ok(new
                {
                    username = User.Identity.Name,
                    lastChanged = User.FindFirst("LastChanged")?.Value
                });
            }

            return Unauthorized();
        }
    }
}

public record LoginRequest(string Username, string Password);