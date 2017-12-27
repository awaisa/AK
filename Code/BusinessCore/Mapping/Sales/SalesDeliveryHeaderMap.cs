using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesDeliveryHeaderMap : BaseEntityMap<SalesDeliveryHeader>
    {
        public override void Configure(EntityTypeBuilder<SalesDeliveryHeader> builder)
        {
            builder.ToTable("SalesDeliveryHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.PaymentTermId).HasColumnName("PaymentTermId");
                builder.HasOne(t => t.PaymentTerm).WithMany().HasForeignKey(t => t.PaymentTermId);

                builder.Property(p => p.CustomerId).HasColumnName("CustomerId");
                builder.HasOne(t => t.Customer).WithMany().HasForeignKey(t => t.CustomerId);

                builder.Property(p => p.GeneralLedgerHeaderId).HasColumnName("GeneralLedgerHeaderId");
                builder.HasOne(t => t.GeneralLedgerHeader).WithMany().HasForeignKey(t => t.GeneralLedgerHeaderId);

                builder.Property(p => p.SalesOrderHeaderId).HasColumnName("SalesOrderHeaderId");
                builder.HasOne(t => t.SalesOrderHeader).WithMany().HasForeignKey(t => t.SalesOrderHeaderId);

                builder.Property(p => p.No).HasColumnName("No").HasMaxLength(20);
                builder.Property(p => p.Date).HasColumnName("Date");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
