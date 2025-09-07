using System;

namespace Models
{
    public class HelpTicket
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Subject { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User? User { get; set; }
    }
}
