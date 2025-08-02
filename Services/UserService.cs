using QUickDish.API.DTOs;
using QUickDish.API.Models;
using QUickDish.API.Repos;

namespace QUickDish.API.Services
{
    public class UserService
    {
        private readonly UserRepo _dbRepo;

        public UserService(UserRepo dbRepo)
        {
            _dbRepo = dbRepo;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbRepo.GetAllUsersAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbRepo.GetUserByIdAsync(id);
        }
        public async Task<User?> GetUserByNameAsync(string name)
        {
            return await _dbRepo.GetUserByNameAsync(name);
        }
        public async Task<string?> GetUserRoleAsync(int id)
        {
            return await _dbRepo.GetUserRoleAsync(id);
        }
        public async Task<bool> EmailExistAsync(string email)
        {
            return await _dbRepo.EmailExistAsync(email);
        }
        public async Task<User> CreateUserAsync(RegisterUserDto dto)
        {
            return await _dbRepo.CreateUserAsync(dto);
        }
        public async Task DeleteUserAsync(int id)
        {
            await _dbRepo.DeleteUserAsync(id);
        }
        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            return await _dbRepo.UpdateUserAsync(id, dto);
        }
        public async Task<User?> AuthenticateUserAsync(LoginDto dto)
        {
            return await _dbRepo.AuthenticateUserAsync(dto);
        }
    }
}
