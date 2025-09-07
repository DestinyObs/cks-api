using System;

namespace DTOs
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string AdminEmail { get; set; } = null!;
        public string Status { get; set; } = "active";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateTenantDto
    {
        public string Name { get; set; } = null!;
        public string AdminEmail { get; set; } = null!;
    }
}
