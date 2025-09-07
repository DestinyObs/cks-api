using System;

namespace Models
{
    public class RbacRolePermission
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }
        public RbacRole? Role { get; set; }
        public RbacPermission? Permission { get; set; }
    }
}
