using QUickDish.API.DTOs;
using QUickDish.API.Models;
using QUickDish.API.Repos;

namespace QUickDish.API.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService(UserRepository dbRepo)
        {
            _userRepo = dbRepo;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepo.GetAllUsersAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepo.GetUserByIdAsync(id);
        }
        public async Task<User?> GetUserByNameAsync(string name)
        {
            return await _userRepo.GetUserByNameAsync(name);
        }
        public async Task<string?> GetUserRoleAsync(int id)
        {
            return await _userRepo.GetUserRoleAsync(id);
        }
        public async Task<bool> EmailExistAsync(string email)
        {
            return await _userRepo.EmailExistAsync(email);
        }
        public async Task<User?> CreateUserAsync(RegisterUserDto dto)
        {
            return await _userRepo.CreateUserAsync(dto);
        }
        public async Task DeleteUserAsync(int id)
        {
            await _userRepo.DeleteUserAsync(id);
        }
        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            return await _userRepo.UpdateUserAsync(id, dto);
        }
    }
}
