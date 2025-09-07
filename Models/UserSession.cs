using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class UserSession
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string? Device { get; set; }
        public string? IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public User? User { get; set; }
    }
}
