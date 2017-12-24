using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class CustomerAllocationMap : BaseEntityMap<CustomerAllocation>
    {
        public override void Configure(EntityTypeBuilder<CustomerAllocation> builder)
        {
            builder.ToTable("CustomerAllocation");

            TableColumns = () =>
            {
                builder.Property(p => p.CustomerId).HasColumnName("CustomerId");
                builder.HasOne(t => t.Customer).WithMany().HasForeignKey(t => t.CustomerId);

                builder.Property(p => p.SalesInvoiceHeaderId).HasColumnName("SalesInvoiceHeaderId");
                builder.HasOne(t => t.SalesInvoiceHeader).WithMany().HasForeignKey(t => t.SalesInvoiceHeaderId);

                builder.Property(p => p.SalesReceiptHeaderId).HasColumnName("SalesReceiptHeaderId");
                builder.HasOne(t => t.SalesReceiptHeader).WithMany().HasForeignKey(t => t.SalesReceiptHeaderId);

                builder.Property(p => p.Date).HasColumnName("Date");
                builder.Property(p => p.Amount).HasColumnName("Amount");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
