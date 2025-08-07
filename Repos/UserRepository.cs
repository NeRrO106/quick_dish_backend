using Microsoft.EntityFrameworkCore;
using QUickDish.API.Data;
using QUickDish.API.DTOs;
using QUickDish.API.Models;

namespace QUickDish.API.Repos
{
    public class UserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User?> GetUserByNameAsync(string name)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name.ToLower() == name.ToLower());
        }
        public async Task<string?> GetUserRoleAsync(int id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => u.Role)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> EmailExistAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower()); //AnyAsync verifica daca exista 
        }
        public async Task<User?> CreateUserAsync(RegisterUserDto dto)
        {
            if (await EmailExistAsync(dto.Email.ToLower()))
                return null;
            var user_exist = await GetUserByNameAsync(dto.Name.ToLower());
            if (user_exist != null)
                return null;
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email.ToLower(),
                Role = "Client",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

            }
        }
        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;
            if (!string.IsNullOrEmpty(dto.Name))
                user.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Role))
                user.Role = dto.Role;
            if (!string.IsNullOrEmpty(dto.PasswordHash))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User?> AuthenticateUserAsync(LoginRequest dto)
        {
            var user = await GetUserByNameAsync(dto.Name);
            if (user == null) return null;
            var result = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!result) return null;
            return user;
        }
    }
}
