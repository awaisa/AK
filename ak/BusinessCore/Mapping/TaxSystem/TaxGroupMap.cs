using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.TaxSystem;

namespace BusinessCore.Mapping.TaxSystem
{
    public partial class TaxGroupMap : BaseEntityMap<TaxGroup>
    {
        public override void Configure(EntityTypeBuilder<TaxGroup> builder)
        {
            builder.ToTable("TaxGroup");

            TableColumns = () =>
            {
                builder.Property(p => p.Description).HasColumnName("Description");
                builder.Property(p => p.TaxAppliedToShipping).HasColumnName("TaxAppliedToShipping");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
