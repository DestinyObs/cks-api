using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Namespace
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Guid ClusterId { get; set; }
        public string? Labels { get; set; } // JSON
        public string? Annotations { get; set; } // JSON
        public string? Quotas { get; set; } // JSON
        public string? Usage { get; set; } // JSON
        public Guid TenantId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Cluster? Cluster { get; set; }
    }
}
