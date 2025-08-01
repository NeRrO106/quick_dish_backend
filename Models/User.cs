﻿namespace QUickDish.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string Role { get; set; } = "Client";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
