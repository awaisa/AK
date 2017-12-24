using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.TaxSystem;

namespace BusinessCore.Mapping.TaxSystem
{
    public partial class ItemTaxGroupMap : BaseEntityMap<ItemTaxGroup>
    {
        public override void Configure(EntityTypeBuilder<ItemTaxGroup> builder)
        {
            builder.ToTable("ItemTaxGroup");

            TableColumns = () =>
            {
                builder.Property(p => p.Name).HasColumnName("Name");
                builder.Property(p => p.IsFullyExempt).HasColumnName("IsFullyExempt");
            };

            base.Configure(builder);
        }
    }
}
