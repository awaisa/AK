using BusinessCore.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Purchases
{
    public partial class PurchaseInvoiceLineMap : BaseEntityMap<PurchaseInvoiceLine>
    {
        public override void Configure(EntityTypeBuilder<PurchaseInvoiceLine> builder)
        {
            builder.ToTable("PurchaseInvoiceLine");

            TableColumns = () =>
            {
                builder.Property(p => p.PurchaseInvoiceHeaderId).HasColumnName("PurchaseInvoiceHeaderId");
                builder.HasOne(t => t.PurchaseInvoiceHeader).WithMany().HasForeignKey(t => t.PurchaseInvoiceHeaderId);

                builder.Property(p => p.ItemId).HasColumnName("ItemId");
                builder.HasOne(t => t.Item).WithMany().HasForeignKey(t => t.ItemId);

                builder.Property(p => p.MeasurementId).HasColumnName("MeasurementId");
                builder.HasOne(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);

                builder.Property(p => p.InventoryControlJournalId).HasColumnName("InventoryControlJournalId");
                builder.HasOne(t => t.InventoryControlJournal).WithMany().HasForeignKey(t => t.InventoryControlJournalId);

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
