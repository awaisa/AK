using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.TaxSystem;

namespace BusinessCore.Mapping.TaxSystem
{
    public partial class ItemTaxGroupTaxMap : BaseEntityMap<ItemTaxGroupTax>
    {
        public override void Configure(EntityTypeBuilder<ItemTaxGroupTax> builder)
        {
            builder.ToTable("ItemTaxGroupTax");

            TableColumns = () =>
            {
                builder.Property(p => p.TaxId).HasColumnName("TaxId");
                builder.HasOne(t => t.Tax).WithMany(t => t.ItemTaxGroupTaxes).HasForeignKey(t => t.TaxId);
                builder.Property(p => p.ItemTaxGroupId).HasColumnName("ItemTaxGroupId");
                builder.HasOne(t => t.ItemTaxGroup).WithMany(t=> t.ItemTaxGroupTax).HasForeignKey(t => t.ItemTaxGroupId);
                builder.Property(p => p.IsExempt).HasColumnName("IsExempt");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
