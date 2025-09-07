using System;

namespace Models
{
    public class Alert
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Severity { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string SourceType { get; set; } = null!;
        public Guid SourceId { get; set; }
        public string? SourceName { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public Guid TenantId { get; set; }
    }
}
