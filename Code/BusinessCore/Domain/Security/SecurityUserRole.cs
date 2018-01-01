namespace BusinessCore.Domain.Security
{
    public class SecurityUserRole : BaseEntity
    {
        public int UserId { get; set; }
        public int SecurityRoleId { get; set; }

        public virtual User User { get; set; }
        public virtual SecurityRole SecurityRole { get; set; }
    }
}
