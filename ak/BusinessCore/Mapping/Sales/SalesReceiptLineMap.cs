using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesReceiptLineMap : BaseEntityMap<SalesReceiptLine>
    {
        public override void Configure(EntityTypeBuilder<SalesReceiptLine> builder)
        {
            builder.ToTable("SalesReceiptLine");

            TableColumns = () =>
            {
                builder.Property(p => p.SalesReceiptHeaderId).HasColumnName("SalesReceiptHeaderId");
                builder.HasOne(t => t.SalesReceiptHeader).WithMany(t=> t.SalesReceiptLines).HasForeignKey(t => t.SalesReceiptHeaderId);

                builder.Property(p => p.SalesInvoiceLineId).HasColumnName("SalesInvoiceLineId");
                builder.HasOne(t => t.SalesInvoiceLine).WithMany().HasForeignKey(t => t.SalesInvoiceLineId);

                builder.Property(p => p.ItemId).HasColumnName("ItemId");
                builder.HasOne(t => t.Item).WithMany().HasForeignKey(t => t.ItemId);

                builder.Property(p => p.AccountToCreditId).HasColumnName("AccountToCreditId");
                builder.HasOne(t => t.AccountToCredit).WithMany().HasForeignKey(t => t.AccountToCreditId);

                builder.Property(p => p.MeasurementId).HasColumnName("MeasurementId");
                builder.HasOne(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);

                builder.Property(p => p.Quantity).HasColumnName("Quantity");
                builder.Property(p => p.Discount).HasColumnName("Discount");
                builder.Property(p => p.Amount).HasColumnName("Amount");
                builder.Property(p => p.AmountPaid).HasColumnName("AmountPaid");
            };

            base.Configure(builder);
        }
    }
}
