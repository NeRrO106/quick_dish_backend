using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QUickDish.API.DTOs;
using QUickDish.API.Services;
using System.Security.Claims;

namespace QUickDish.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user data.");
            var user = await _userService.CreateUserAsync(dto);
            return Ok(user);
        }//permisiune admin
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            var user = await _userService.UpdateUserAsync(id, dto);
            if (!user)
                return NotFound("User not found.");
            return Ok(user);
        }
        //permisiune admin
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok("User deleted successfully.");

        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto dto)
        {
            var user = await _userService.AuthenticateUserAsync(dto);
            if (user == null)
                return Unauthorized("Invalid credential");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
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
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var name = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok(new { name, email, role });
        }

    }
}
