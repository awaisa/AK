namespace BusinessCore.Domain.Security
{
    public class SecurityRolePermission : BaseEntity
    {
        public int SecurityRoleId { get; set; }
        public int SecurityPermissionId { get; set; }

        public virtual SecurityRole SecurityRole { get; set; }
        public virtual SecurityPermission SecurityPermission { get; set; }
    }
}
