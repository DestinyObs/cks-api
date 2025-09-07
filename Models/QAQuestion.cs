using System;
using System.Collections.Generic;

namespace Models
{
    public class QAQuestion
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Question { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    // public ICollection<QAAnswer> Answers { get; set; } = new List<QAAnswer>(); // QAAnswer model temporarily removed
        public User? User { get; set; }
    }
}
