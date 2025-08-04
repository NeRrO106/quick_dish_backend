using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using QUickDish.API.DTOs;
using QUickDish.API.Services;
using System.Security.Claims;

namespace QUickDish.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthServices _authServices;
        public AuthController(AuthServices authServices)
        {
            _authServices = authServices;
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest dto)
        {
            var user = await _authServices.AuthenticateUserAsync(dto);
            if (user == null)
                return Unauthorized("Invalid credential");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(2)
            });

            return Ok("Login succesful");
        }
        [HttpPost("login-ghost")]
        public async Task<IActionResult> LoginGuest()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, "Ghost"),
                new Claim(ClaimTypes.Email, "ghost@ghost.com"),
                new Claim(ClaimTypes.Role, "Ghost"),
            };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(2)
            });

            return Ok("Login succesful as a ghost");
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Ok("SignOut");
        }
    }
}
