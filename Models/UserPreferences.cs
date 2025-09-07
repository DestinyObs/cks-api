using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class UserPreferences
    {
        [Key, ForeignKey("User")]
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public string Notifications { get; set; } = "{}"; // JSON
        public string Dashboard { get; set; } = "{}"; // JSON
        public User? User { get; set; }
    }
}
