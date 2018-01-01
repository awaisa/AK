using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Security;

namespace BusinessCore.Mapping.Security
{
    public class SecurityRoleMap : BaseEntityMap<SecurityRole>
    {
        public override void Configure(EntityTypeBuilder<SecurityRole> builder)
        {
            builder.ToTable("SecurityRole");

            TableColumns = () =>
            {
                builder.Property(p => p.RoleName).HasColumnName("RoleName").HasMaxLength(100);
                builder.Property(p => p.SysAdmin).HasColumnName("SysAdmin");
            };

            base.Configure(builder);
        }
    }
}
