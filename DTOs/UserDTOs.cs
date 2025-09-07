using System;
using System.Collections.Generic;

namespace DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Role { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime? LastLogin { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateUserDto
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Password { get; set; } = null!;
    }

    public class UpdateUserDto
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Avatar { get; set; }
        public List<string>? Permissions { get; set; }
    }
}
