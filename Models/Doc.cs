using System;

namespace Models
{
    public class Doc
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Category { get; set; }
        public string? Tags { get; set; } // Comma-separated or JSON
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
