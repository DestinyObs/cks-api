using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class UserToken
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUsed { get; set; }
        public User? User { get; set; }
    }
}
