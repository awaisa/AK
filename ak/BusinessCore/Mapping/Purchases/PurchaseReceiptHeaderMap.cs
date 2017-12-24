using BusinessCore.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Purchases
{
    public partial class PurchaseReceiptHeaderMap : BaseEntityMap<PurchaseReceiptHeader>
    {
        public override void Configure(EntityTypeBuilder<PurchaseReceiptHeader> builder)
        {
            builder.ToTable("PurchaseReceiptHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.VendorId).HasColumnName("VendorId");
                builder.HasOne(t => t.Vendor).WithMany(t => t.PurchaseReceipts).HasForeignKey(t => t.VendorId);

                builder.Property(p => p.PurchaseOrderHeaderId).HasColumnName("PurchaseOrderHeaderId");
                builder.HasOne(t => t.PurchaseOrderHeader).WithMany(t => t.PurchaseReceipts).HasForeignKey(t => t.PurchaseOrderHeaderId);

                builder.Property(p => p.GeneralLedgerHeaderId).HasColumnName("GeneralLedgerHeaderId");
                builder.HasOne(t => t.GeneralLedgerHeader).WithMany(t => t.PurchaseOrderReceipts).HasForeignKey(t => t.GeneralLedgerHeaderId);

                builder.Property(p => p.Date).HasColumnName("Date");
                builder.Property(p => p.No).HasColumnName("No");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
