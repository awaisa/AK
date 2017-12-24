using BusinessCore.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Purchases
{
    public partial class PurchaseReceiptLineMap : BaseEntityMap<PurchaseReceiptLine>
    {
        public override void Configure(EntityTypeBuilder<PurchaseReceiptLine> builder)
        {
            builder.ToTable("PurchaseReceiptLine");

            TableColumns = () =>
            {
                builder.Property(p => p.PurchaseReceiptHeaderId).HasColumnName("PurchaseReceiptHeaderId");
                builder.HasOne(t => t.PurchaseReceiptHeader).WithMany(t => t.PurchaseReceiptLines).HasForeignKey(t => t.PurchaseReceiptHeaderId);

                builder.Property(p => p.ItemId).HasColumnName("ItemId");
                builder.HasOne(t => t.Item).WithMany(t => t.PurchaseReceiptLines).HasForeignKey(t => t.ItemId);

                builder.Property(p => p.TaxId).HasColumnName("TaxId");
                builder.HasOne(t => t.Tax).WithMany().HasForeignKey(t => t.TaxId);


                builder.Property(p => p.InventoryControlJournalId).HasColumnName("InventoryControlJournalId");
                builder.HasOne(t => t.InventoryControlJournal).WithMany().HasForeignKey(t => t.InventoryControlJournalId);


                builder.Property(p => p.PurchaseOrderLineId).HasColumnName("PurchaseOrderLineId");
                builder.HasOne(t => t.PurchaseOrderLine).WithMany(t => t.PurchaseReceiptLines).HasForeignKey(t => t.PurchaseOrderLineId);


                builder.Property(p => p.MeasurementId).HasColumnName("MeasurementId");
                builder.HasOne(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);



                builder.Property(p => p.Quantity).HasColumnName("Quantity");
                builder.Property(p => p.ReceivedQuantity).HasColumnName("ReceivedQuantity");
                builder.Property(p => p.Cost).HasColumnName("Cost");
                builder.Property(p => p.Discount).HasColumnName("Discount");
                builder.Property(p => p.Amount).HasColumnName("Amount");
            };

            base.Configure(builder);
        }
    }
}
