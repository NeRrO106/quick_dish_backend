namespace QUickDish.API.DTOs
{
    public class LoginRequest
    {
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}
