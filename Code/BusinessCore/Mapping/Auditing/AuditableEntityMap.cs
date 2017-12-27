using BusinessCore.Domain.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Auditing
{
    public class AuditableEntityMap : BaseEntityMap<AuditableEntity>
    {
        public override void Configure(EntityTypeBuilder<AuditableEntity> builder)
        {
            builder.ToTable("AuditableEntity");

            TableColumns = () =>
            {
                builder.Property(p => p.EntityName).HasColumnName("EntityName");
                builder.Property(p => p.EnableAudit).HasColumnName("EnableAudit");
            };

            base.Configure(builder);
        }

    }
}
