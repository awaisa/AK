using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Security;

namespace BusinessCore.Mapping.Security
{
    public class SecurityGroupMap : BaseEntityMap<SecurityGroup>
    {
        public override void Configure(EntityTypeBuilder<SecurityGroup> builder)
        {
            builder.ToTable("SecurityGroup");

            TableColumns = () =>
            {
                builder.Property(p => p.GroupName).HasColumnName("GroupName");
            };

            base.Configure(builder);
        }
    }
}
