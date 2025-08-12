using QUickDish.API.DTOs;
using QUickDish.API.Models;
using QUickDish.API.Repos;

namespace QUickDish.API.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;

        public AuthService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateUserAsync(LoginRequest dto)
        {
            var user = await _userRepository.GetUserByNameAsync(dto.Name);
            if (user == null) return null;
            var result = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!result) return null;
            return user;
        }
    }
}
