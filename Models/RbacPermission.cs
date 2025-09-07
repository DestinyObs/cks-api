using System;
using System.Collections.Generic;

namespace Models
{
    public class RbacPermission
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<RbacRolePermission> RolePermissions { get; set; } = new List<RbacRolePermission>();
    }
}
