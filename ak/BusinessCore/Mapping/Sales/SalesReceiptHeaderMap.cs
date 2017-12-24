using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesReceiptHeaderMap : BaseEntityMap<SalesReceiptHeader>
    {
        public override void Configure(EntityTypeBuilder<SalesReceiptHeader> builder)
        {
            builder.ToTable("SalesReceiptHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.CustomerId).HasColumnName("CustomerId");
                builder.HasOne(t => t.Customer).WithMany().HasForeignKey(t => t.CustomerId);

                builder.Property(p => p.GeneralLedgerHeaderId).HasColumnName("GeneralLedgerHeaderId");
                builder.HasOne(t => t.GeneralLedgerHeader).WithMany().HasForeignKey(t => t.GeneralLedgerHeaderId);

                builder.Property(p => p.AccountToDebitId).HasColumnName("AccountToDebitId");
                builder.HasOne(t => t.AccountToDebit).WithMany().HasForeignKey(t => t.AccountToDebitId);

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
