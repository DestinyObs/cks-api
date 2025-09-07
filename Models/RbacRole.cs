using System;
using System.Collections.Generic;

namespace Models
{
    public class RbacRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Status { get; set; } = "active";
        public ICollection<RbacRolePermission> RolePermissions { get; set; } = new List<RbacRolePermission>();
        public ICollection<RbacUserRole> UserRoles { get; set; } = new List<RbacUserRole>();
    }
}
