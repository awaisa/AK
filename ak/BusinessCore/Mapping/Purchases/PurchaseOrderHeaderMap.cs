using BusinessCore.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Purchases
{
    public partial class PurchaseOrderHeaderMap : BaseEntityMap<PurchaseOrderHeader>
    {
        public override void Configure(EntityTypeBuilder<PurchaseOrderHeader> builder)
        {
            builder.ToTable("PurchaseOrderHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.VendorId).HasColumnName("VendorId");
                builder.HasOne(t => t.Vendor).WithMany(t=> t.PurchaseOrders).HasForeignKey(t => t.VendorId);

                builder.Property(p => p.PurchaseInvoiceHeaderId).HasColumnName("PurchaseInvoiceHeaderId");
                builder.HasOne(t => t.PurchaseInvoiceHeader).WithMany(t=> t.PurchaseOrders).HasForeignKey(t => t.PurchaseInvoiceHeaderId);

                builder.Property(p => p.No).HasColumnName("No").HasMaxLength(20);
                builder.Property(p => p.Date).HasColumnName("Date");
                builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(500);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
