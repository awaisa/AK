using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Security;

namespace BusinessCore.Mapping.Security
{
    public class SecurityPermissionMap : BaseEntityMap<SecurityPermission>
    {
        public override void Configure(EntityTypeBuilder<SecurityPermission> builder)
        {
            builder.ToTable("SecurityPermission");

            TableColumns = () =>
            {
                builder.Property(p => p.PermissionName).HasColumnName("PermissionName").HasMaxLength(100);
                builder.Property(p => p.DisplayName).HasColumnName("DisplayName").HasMaxLength(100);

                builder.Property(p => p.SecurityGroupId).HasColumnName("SecurityGroupId");
                builder.HasOne(t => t.Groups).WithMany(g => g.Permissions).HasForeignKey(t => t.SecurityGroupId);
            };

            base.Configure(builder);
        }
    }
}
