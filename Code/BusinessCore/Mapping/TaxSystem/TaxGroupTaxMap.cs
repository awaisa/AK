using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.TaxSystem;

namespace BusinessCore.Mapping.TaxSystem
{
    public partial class TaxGroupTaxMap : BaseEntityMap<TaxGroupTax>
    {
        public override void Configure(EntityTypeBuilder<TaxGroupTax> builder)
        {
            builder.ToTable("TaxGroupTax");

            TableColumns = () =>
            {
                builder.Property(p => p.TaxId).HasColumnName("TaxId");
                builder.HasOne(t => t.Tax).WithMany(t => t.TaxGroupTaxes).HasForeignKey(t => t.TaxId);

                builder.Property(p => p.TaxGroupId).HasColumnName("TaxGroupId");
                builder.HasOne(t => t.TaxGroup).WithMany(t => t.TaxGroupTax).HasForeignKey(t => t.TaxGroupId);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
