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

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid user data.");
            var user = await _userService.CreateUserAsync(dto);
            return Ok(user);
        }//permisiune admin
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            var user = await _userService.UpdateUserAsync(id, dto);
            if (!user)
                return NotFound("User not found.");
            return Ok(user);
        }
        //permisiune admin
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok("User deleted successfully.");

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
