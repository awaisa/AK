using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Security;

namespace BusinessCore.Mapping.Security
{
    public class SecurityUserRoleMap : BaseEntityMap<SecurityUserRole>
    {
        public override void Configure(EntityTypeBuilder<SecurityUserRole> builder)
        {
            builder.ToTable("SecurityUserRole");

            TableColumns = () =>
            {
                builder.Property(p => p.UserId).HasColumnName("UserId");
                builder.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId);

                builder.Property(p => p.SecurityRoleId).HasColumnName("SecurityRoleId");
                builder.HasOne(t => t.SecurityRole).WithMany().HasForeignKey(t => t.SecurityRoleId);

            };

            base.Configure(builder);
        }
    }
}
