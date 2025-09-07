using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class NodePool
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ClusterId { get; set; }
        public string Name { get; set; } = null!;
        public int NodeCount { get; set; }
        public int MinNodes { get; set; }
        public int MaxNodes { get; set; }
        public string InstanceType { get; set; } = null!;
        public int DiskSize { get; set; }
        public string? Labels { get; set; } // JSON
        public string? Taints { get; set; } // JSON
        public Cluster? Cluster { get; set; }
    }
}
