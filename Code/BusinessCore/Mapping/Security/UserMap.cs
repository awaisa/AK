using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Security;

namespace BusinessCore.Mapping.Security
{
    public class UserMap : BaseEntityMap<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            TableColumns = () =>
            {
                builder.Property(p => p.Username).HasColumnName("Username").HasMaxLength(100);
                builder.Property(p => p.Firstname).HasColumnName("Firstname").HasMaxLength(100);
                builder.Property(p => p.Lastname).HasColumnName("Lastname").HasMaxLength(100);
                builder.Property(p => p.Password).HasColumnName("Password").HasMaxLength(100);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
