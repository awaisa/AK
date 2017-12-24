using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesInvoiceHeaderMap : BaseEntityMap<SalesInvoiceHeader>
    {
        public override void Configure(EntityTypeBuilder<SalesInvoiceHeader> builder)
        {
            builder.ToTable("SalesInvoiceHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.CustomerId).HasColumnName("CustomerId");
                builder.HasOne(t => t.Customer).WithMany().HasForeignKey(t => t.CustomerId);

                builder.Property(p => p.GeneralLedgerHeaderId).HasColumnName("GeneralLedgerHeaderId");
                builder.HasOne(t => t.GeneralLedgerHeader).WithMany().HasForeignKey(t => t.GeneralLedgerHeaderId);

                builder.Property(p => p.SalesDeliveryHeaderId).HasColumnName("SalesDeliveryHeaderId");
                builder.HasOne(t => t.SalesDeliveryHeader).WithMany().HasForeignKey(t => t.SalesDeliveryHeaderId);

                builder.Property(p => p.No).HasColumnName("No");
                builder.Property(p => p.Date).HasColumnName("Date");
                builder.Property(p => p.ShippingHandlingCharge).HasColumnName("ShippingHandlingCharge");
                builder.Property(p => p.Status).HasColumnName("Status");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
