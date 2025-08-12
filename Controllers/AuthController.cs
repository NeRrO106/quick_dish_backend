using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly UserService _userService;
        private readonly IMemoryCache _cache;

        private Random _random = new Random();

        public AuthController(AuthService authServices, EmailService emailService, IMemoryCache cache, UserService userService)
        {
            _authServices = authServices;
            _emailService = emailService;
            _userService = userService;
            _cache = cache;
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
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return BadRequest("Invalid request");
            var user = await _userService.EmailExistAsync(Email.ToLower());
            if (!user)
                return NotFound("User not found");
            var code = _random.Next(100000, 999999).ToString();

            _cache.Set(Email, code, TimeSpan.FromMinutes(10));

            await _emailService.SendEmailAsync(
                Email,
                "Password Reset Code",
                $"<h1>Password Reset Code</h1> <p>Your password reset code is: <strong>{code}</strong></p>"
            );
            return Ok("Code send");

        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest dto)
        {
            if (string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Code) || string.IsNullOrEmpty(dto.NewPassword))
                return BadRequest("Invalid request");
            var userExists = await _userService.EmailExistAsync(dto.Email.ToLower());
            if (!userExists)
                return NotFound("User not found");
            if (!_cache.TryGetValue(dto.Email, out string? cachedCode) || cachedCode != dto.Code)
                return BadRequest("Invalid code or code expired");
            var result = await _userService.ChangePasswordAsync(dto.Email, dto.NewPassword);
            if (!result)
                return BadRequest("Failed to reset password");
            _cache.Remove(dto.Email);

            await _emailService.SendEmailAsync(
                dto.Email,
                "Password Reset Confirmation",
                $"<h1>Password Reset Successful</h1> <p>Your password has been reset successfully.</p> <p>Your new password is {dto.NewPassword}</p>"
            );

            return Ok("Password reset successfully");
        }
    }
}
