using QUickDish.API.DTOs;
using QUickDish.API.Models;
using QUickDish.API.Repos;
using System.Text.RegularExpressions;

namespace QUickDish.API.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepo;

        public UserService(UserRepository dbRepo)
        {
            _userRepo = dbRepo;
        }

        private static bool IsValidEmail(string email) => Regex.IsMatch(email, @"^[\w\.\-]+@([\w\-]+\.)+[\w\-]{2,}$");

        private static bool IsValidPassword(string password) => !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");

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
            string lowerName = name.ToLower();
            return await _userRepo.GetUserByNameAsync(lowerName);
        }
        public async Task<string?> GetUserRoleAsync(int id)
        {
            return await _userRepo.GetUserRoleAsync(id);
        }
        public async Task<bool> EmailExistAsync(string email)
        {
            return await _userRepo.EmailExistAsync(email);
        }
        public async Task<User?> CreateUserAsync(RegisterUserRequest dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Password))
                return null;

            if (!IsValidEmail(dto.Email) || await _userRepo.EmailExistAsync(dto.Email))
                return null;

            if (!IsValidPassword(dto.Password))
                return null;

            if (await _userRepo.GetUserByNameAsync(dto.Name) != null)
                return null;

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email.ToLower(),
                Role = dto.Role ?? "Client",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };
            await _userRepo.CreateUserAsync(user);
            return user;
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepo.GetUserByIdAsync(id);
            if (user != null)
            {
                await _userRepo.DeleteUserAsync(user);
            }
        }
        public async Task<bool> UpdateUserAsync(int id, UserUpdateRequest dto)
        {
            var user = await _userRepo.GetUserByIdAsync(id);

            if (user == null)
                return false;

            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                if (await _userRepo.EmailExistAsync(dto.Email) || !IsValidEmail(dto.Email))
                    return false;
                user.Email = dto.Email.ToLower();
            }

            if (!string.IsNullOrEmpty(dto.Role))
                user.Role = dto.Role;

            if (IsValidPassword(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _userRepo.UpdateUserAsync(user);
            return true;
        }

        public async Task<bool> ChangePasswordAsync(string email, string newPassword)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            if (user == null)
                return false;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepo.UpdateUserAsync(user);
            return true;
        }
    }
}
