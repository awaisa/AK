namespace BusinessCore.Domain.Auditing
{
    public class AuditableAttribute : BaseEntity
    {
        public int AuditableEntityId { get; set; }
        public string AttributeName { get; set; }
        public bool EnableAudit { get; set; }
        public virtual AuditableEntity AuditableEntity { get; set; }
    }
}
