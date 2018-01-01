
using BusinessCore.Domain.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Auditing
{
    public class AuditLogMap : BaseEntityMap<AuditLog>
    {
        public override void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLog");

            TableColumns = () =>
            {
                builder.Property(p => p.UserName).HasColumnName("UserName").HasMaxLength(100);
                builder.Property(p => p.AuditEventDateUTC).HasColumnName("AuditEventDateUTC");
                builder.Property(p => p.AuditEventType).HasColumnName("AuditEventType");
                builder.Property(p => p.TableName).HasColumnName("TableName").HasMaxLength(100);
                builder.Property(p => p.RecordId).HasColumnName("RecordId");
                builder.Property(p => p.FieldName).HasColumnName("FieldName").HasMaxLength(100);
                builder.Property(p => p.OriginalValue).HasColumnName("OriginalValue");
                builder.Property(p => p.NewValue).HasColumnName("NewValue");
            };

            base.Configure(builder);
        }
    }
}
