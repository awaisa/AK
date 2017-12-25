using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesInvoiceLineMap : BaseEntityMap<SalesInvoiceLine>
    {
        public override void Configure(EntityTypeBuilder<SalesInvoiceLine> builder)
        {
            builder.ToTable("SalesInvoiceLine");

            TableColumns = () =>
            {
                builder.Property(p => p.SalesInvoiceHeaderId).HasColumnName("SalesInvoiceHeaderId");
                builder.HasOne(t => t.SalesInvoiceHeader).WithMany(t=> t.SalesInvoiceLines).HasForeignKey(t => t.SalesInvoiceHeaderId);

                builder.Property(p => p.ItemId).HasColumnName("ItemId");
                builder.HasOne(t => t.Item).WithMany(t => t.SalesInvoiceLines).HasForeignKey(t => t.ItemId);

                builder.Property(p => p.MeasurementId).HasColumnName("MeasurementId");
                builder.HasOne(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);

                builder.Property(p => p.InventoryControlJournalId).HasColumnName("InventoryControlJournalId");
                builder.HasOne(t => t.InventoryControlJournal).WithMany().HasForeignKey(t => t.InventoryControlJournalId);

                builder.Property(p => p.TaxId).HasColumnName("TaxId");
                builder.HasOne(t => t.Tax).WithMany().HasForeignKey(t => t.TaxId);

                builder.Property(p => p.Quantity).HasColumnName("Quantity");
                builder.Property(p => p.Discount).HasColumnName("Discount");
                builder.Property(p => p.Amount).HasColumnName("Amount");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
