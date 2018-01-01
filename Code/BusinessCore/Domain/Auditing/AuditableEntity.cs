using System.Collections.Generic;

namespace BusinessCore.Domain.Auditing
{
    public class AuditableEntity : BaseEntity
    {
        public string EntityName { get; set; }
        public bool EnableAudit { get; set; }
        public virtual ICollection<AuditableAttribute> AuditableAttributes { get; set; }

        public AuditableEntity()
        {
            AuditableAttributes = new HashSet<AuditableAttribute>();
        }
    }
}
