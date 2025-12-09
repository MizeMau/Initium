using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        [HttpPost("user")]
        public IActionResult User_Post([FromBody] Database.Table.Backend.User.Model user)
        {
            var dbTableBackendUserService = new Database.Table.Backend.User.Service();
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            user.Salt = Convert.ToBase64String(salt);
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            user.Password = hashedPassword;
            var dbUser = dbTableBackendUserService.Create(user);
            return Ok(dbUser);
        }

        [HttpGet("user/login")]
        public async Task<IActionResult> User_Login_GET(string username, string password)
        {
            var dbTableBackendUserService = new Database.Table.Backend.User.Service();
            var user = dbTableBackendUserService.GetByUsername(username);
            byte[] salt = Convert.FromBase64String(user!.Salt);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            if (user.Password != hashedPassword) 
                return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.BackendUserID.ToString()),
                new Claim(ClaimTypes.Name, username),
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

            var userDTO = new Database.Table.Backend.User.DTO(user);
            return Ok(userDTO);
        }

        [HttpGet("user/logout")]
        public async Task<IActionResult> User_Logout_GET()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logged out" });
        }

        [HttpGet("user/currentUser")]
        public IActionResult User_CurrentUser_GET()
        {
            if (!(User.Identity?.IsAuthenticated ?? false))
                return Unauthorized();

            var dbTableBackendUserService = new Database.Table.Backend.User.Service();
            var userIDString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIDString == null)
                return Unauthorized();

            var userID = long.Parse(userIDString);
            var user = dbTableBackendUserService.GetById(userID)!;
            if (user.Deleted != null)
                return Unauthorized();

            var userDTO = new Database.Table.Backend.User.DTO(user);
            return Ok(userDTO);
        }
    }
}