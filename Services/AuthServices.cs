using QUickDish.API.DTOs;
using QUickDish.API.Models;
using QUickDish.API.Repos;

namespace QUickDish.API.Services
{
    public class AuthServices
    {
        private readonly UserRepository _userRepository;

        public AuthServices(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateUserAsync(LoginRequest dto)
        {
            return await _userRepository.AuthenticateUserAsync(dto);
        }
    }
}
