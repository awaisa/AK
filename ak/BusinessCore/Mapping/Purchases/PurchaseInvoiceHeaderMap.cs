using BusinessCore.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Purchases
{
    public partial class PurchaseInvoiceHeaderMap : BaseEntityMap<PurchaseInvoiceHeader>
    {
        public override void Configure(EntityTypeBuilder<PurchaseInvoiceHeader> builder)
        {
            builder.ToTable("PurchaseInvoiceHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.VendorId).HasColumnName("VendorId");
                builder.HasOne(t => t.Vendor).WithMany(t => t.PurchaseInvoices).HasForeignKey(t => t.VendorId);

                builder.Property(p => p.GeneralLedgerHeaderId).HasColumnName("GeneralLedgerHeaderId");
                builder.HasOne(t => t.GeneralLedgerHeader).WithMany().HasForeignKey(t => t.GeneralLedgerHeaderId);

                builder.Property(p => p.Date).HasColumnName("Date");
                builder.Property(p => p.No).HasColumnName("No").HasMaxLength(20);
                builder.Property(p => p.VendorInvoiceNo).HasColumnName("VendorInvoiceNo").HasMaxLength(50);
                builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(500);
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
