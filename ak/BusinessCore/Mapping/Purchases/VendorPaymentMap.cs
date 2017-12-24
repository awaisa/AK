using BusinessCore.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Purchases
{
    public partial class VendorPaymentMap : BaseEntityMap<VendorPayment>
    {
        public override void Configure(EntityTypeBuilder<VendorPayment> builder)
        {
            builder.ToTable("VendorPayment");

            TableColumns = () =>
            {
                builder.Property(p => p.VendorId).HasColumnName("VendorId");
                builder.HasOne(t => t.Vendor).WithMany(t => t.VendorPayments).HasForeignKey(t => t.VendorId);

                builder.Property(p => p.PurchaseInvoiceHeaderId).HasColumnName("PurchaseInvoiceHeaderId");
                builder.HasOne(t => t.PurchaseInvoiceHeader).WithMany(t => t.VendorPayments).HasForeignKey(t => t.PurchaseInvoiceHeaderId);

                builder.Property(p => p.GeneralLedgerHeaderId).HasColumnName("GeneralLedgerHeaderId");
                builder.HasOne(t => t.GeneralLedgerHeader).WithMany().HasForeignKey(t => t.GeneralLedgerHeaderId);

                builder.Property(p => p.No).HasColumnName("No");
                builder.Property(p => p.Date).HasColumnName("Date");
                builder.Property(p => p.Amount).HasColumnName("Amount");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
