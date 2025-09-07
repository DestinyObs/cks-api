using System;

namespace Models
{
    public class RbacUserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public Guid? ClusterId { get; set; }
        public Guid? NamespaceId { get; set; }
        public User? User { get; set; }
        public RbacRole? Role { get; set; }
    }
}
