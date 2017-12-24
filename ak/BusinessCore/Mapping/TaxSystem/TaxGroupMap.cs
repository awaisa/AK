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
            };

            base.Configure(builder);
        }
    }
}
