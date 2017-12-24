using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesDeliveryLineMap : BaseEntityMap<SalesDeliveryLine>
    {
        public override void Configure(EntityTypeBuilder<SalesDeliveryLine> builder)
        {
            builder.ToTable("SalesDeliveryLine");

            TableColumns = () =>
            {
                builder.Property(p => p.SalesDeliveryHeaderId).HasColumnName("SalesDeliveryHeaderId");
                builder.HasOne(t => t.SalesDeliveryHeader).WithMany(t=> t.SalesDeliveryLines).HasForeignKey(t => t.SalesDeliveryHeaderId);

                builder.Property(p => p.ItemId).HasColumnName("ItemId");
                builder.HasOne(t => t.Item).WithMany().HasForeignKey(t => t.ItemId);

                builder.Property(p => p.MeasurementId).HasColumnName("MeasurementId");
                builder.HasOne(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);

                builder.Property(p => p.Quantity).HasColumnName("Quantity");
                builder.Property(p => p.Price).HasColumnName("Price");
                builder.Property(p => p.Discount).HasColumnName("Discount");
            };

            base.Configure(builder);
        }
    }
}
