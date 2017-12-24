using BusinessCore.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Purchases
{
    public partial class PurchaseOrderLineMap : BaseEntityMap<PurchaseOrderLine>
    {
        public override void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
        {
            builder.ToTable("PurchaseOrderLine");

            TableColumns = () =>
            {
                builder.Property(p => p.PurchaseOrderHeaderId).HasColumnName("PurchaseOrderHeaderId");
                builder.HasOne(t => t.PurchaseOrderHeader).WithMany().HasForeignKey(t => t.PurchaseOrderHeaderId);

                builder.Property(p => p.ItemId).HasColumnName("ItemId");
                builder.HasOne(t => t.Item).WithMany().HasForeignKey(t => t.ItemId);

                builder.Property(p => p.MeasurementId).HasColumnName("MeasurementId");
                builder.HasOne(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);

                builder.Property(p => p.Quantity).HasColumnName("Quantity");
                builder.Property(p => p.Cost).HasColumnName("Cost");
                builder.Property(p => p.Discount).HasColumnName("Discount");
                builder.Property(p => p.Amount).HasColumnName("Amount");
            };

            base.Configure(builder);
        }
    }
}
