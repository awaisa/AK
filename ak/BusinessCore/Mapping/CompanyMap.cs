using BusinessCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping
{
    public partial class CompanyMap : BaseEntityMap<Company>
    {
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Company");

            TableColumns = () =>
            {
                builder.Property(p => p.Name).HasColumnName("Name");
                builder.Property(p => p.ShortName).HasColumnName("ShortName");
                builder.Property(p => p.CompanyCode).HasColumnName("CompanyCode");
            };

            base.Configure(builder);
        }
    }
}
