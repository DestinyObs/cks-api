using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Cluster
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string Version { get; set; } = null!;
        public string Networking { get; set; } = "{}"; // JSON
        public string? Tags { get; set; } // JSON
        public Guid TenantId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<NodePool> NodePools { get; set; } = new List<NodePool>();
        public ICollection<Namespace> Namespaces { get; set; } = new List<Namespace>();
    }
}
