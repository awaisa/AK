using System.Collections.Generic;

namespace BusinessCore.Domain.Security
{
    public class SecurityPermission : BaseEntity
    {
        public string PermissionName { get; set; }
        public string DisplayName { get; set; }
        public int SecurityGroupId { get; set; }

        public virtual SecurityGroup Groups { get; set; }
        public virtual ICollection<SecurityRolePermission> RolePermission { get; set; }

        public SecurityPermission()
        {
            RolePermission = new HashSet<SecurityRolePermission>();
        }
    }
}
