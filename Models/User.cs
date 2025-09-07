using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Models
{
    public class User : IdentityUser<Guid>
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; } = null!;
        public string? Avatar { get; set; }
        public string Role { get; set; } = null!; // 'admin' | 'developer' | 'viewer'
        public string Status { get; set; } = "active"; // 'active' | 'inactive' | 'suspended'
        public DateTime? LastLogin { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public UserPreferences? Preferences { get; set; }
        public ICollection<UserToken> Tokens { get; set; } = new List<UserToken>();
        public ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
        public ICollection<RbacUserRole> UserRoles { get; set; } = new List<RbacUserRole>();
    }
}
