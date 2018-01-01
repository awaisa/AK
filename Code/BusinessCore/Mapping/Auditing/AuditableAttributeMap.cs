using BusinessCore.Domain.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Auditing
{
    public class AuditableAttributeMap : BaseEntityMap<AuditableAttribute>
    {
        public override void Configure(EntityTypeBuilder<AuditableAttribute> builder)
        {
            builder.ToTable("AuditableAttribute");

            TableColumns = () =>
            {
                builder.Property(p => p.AuditableEntityId).HasColumnName("AuditableEntityId");
                builder.HasOne(t => t.AuditableEntity).WithMany(t => t.AuditableAttributes).HasForeignKey(t => t.AuditableEntityId);
                builder.Property(p => p.AttributeName).HasColumnName("AttributeName").HasMaxLength(100);
                builder.Property(p => p.EnableAudit).HasColumnName("EnableAudit");
            };

            base.Configure(builder);
        }
    }
}
