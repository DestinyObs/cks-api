using System;

namespace Models
{
    public class Operation
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string ResourceType { get; set; } = null!;
        public Guid ResourceId { get; set; }
        public string? ResourceName { get; set; }
        public int Progress { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Error { get; set; }
        public Guid TenantId { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
