using System;

namespace Models
{
    public class ActivityLog
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Action { get; set; } = null!;
        public string? ResourceType { get; set; }
        public Guid? ResourceId { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Status { get; set; }
        public User? User { get; set; }
    }
}
