namespace QUickDish.API.DTOs
{
    public class UserUpdateRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public string? PasswordHash { get; set; }
    }
}
