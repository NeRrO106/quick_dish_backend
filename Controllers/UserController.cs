using Microsoft.AspNetCore.Mvc;
using QUickDish.API.DTOs;
using QUickDish.API.Services;

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
            if (await _userService.EmailExistAsync(dto.Email))
                return BadRequest("Email already exists.");
            var user = await _userService.CreateUserAsync(dto);
            return Ok(user);
        }
        [HttpPut("UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto dto)
        {
            var user = await _userService.UpdateUserAsync(id, dto);
            if (!user)
                return NotFound("User not found.");
            return Ok(user);
        }
        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok("User deleted successfully.");

        }
        /*[HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto dto)
        {
            var user = await _userService.GetUserByNameAsync(dto.Name);
            if (user == null || !BCrypt.Net.BCrypt.Verify(BCrypt.Net.BCrypt.HashPassword(dto.Password), user.PasswordHash))
                return Unauthorized("Invalid username or password.");
            var claims = new List<Claim>
            {

            }

        }*/

    }
}
