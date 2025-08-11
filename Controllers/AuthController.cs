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
        private readonly AuthService _authServices;
        private readonly EmailService _emailService;
        public AuthController(AuthService authServices, EmailService emailService)
        {
            _authServices = authServices;
            _emailService = emailService;
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

        [HttpGet("send")]
        public async Task<IActionResult> SendEmail()
        {
            try
            {
                await _emailService.SendEmailAsync(
                    "andreilimit66@gmail.com",
                    "\"Test Email from QuickDish\"",
                     "<h1>Hello from QuickDish</h1><p>This is a test email sent from the QuickDish application.</p>"
                );
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to send email: {ex.Message}");


            }
        }
    }
}
