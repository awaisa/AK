using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Security;

namespace BusinessCore.Mapping.Security
{
    public class SecurityRolePermissionMap : BaseEntityMap<SecurityRolePermission>
    {
        public override void Configure(EntityTypeBuilder<SecurityRolePermission> builder)
        {
            builder.ToTable("SecurityRolePermission");

            TableColumns = () =>
            {
                builder.Property(p => p.SecurityRoleId).HasColumnName("SequenceNumberType");
                builder.HasOne(t => t.SecurityRole).WithMany().HasForeignKey(t => t.SecurityRoleId);

                builder.Property(p => p.SecurityPermissionId).HasColumnName("SecurityPermissionId");
                builder.HasOne(t => t.SecurityPermission).WithMany().HasForeignKey(t => t.SecurityPermissionId);
            };

            base.Configure(builder);
        }
    }
}
