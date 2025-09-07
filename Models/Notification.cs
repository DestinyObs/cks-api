using System;

namespace Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Type { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool Read { get; set; }
        public DateTime CreatedAt { get; set; }
        public User? User { get; set; }
    }
}
