using System;
using System.Collections.Generic;

namespace Models
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string AdminEmail { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
