using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesQuoteHeaderMap : BaseEntityMap<SalesQuoteHeader>
    {
        public override void Configure(EntityTypeBuilder<SalesQuoteHeader> builder)
        {
            builder.ToTable("SalesQuoteHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.CustomerId).HasColumnName("CustomerId");
                builder.HasOne(t => t.Customer).WithMany().HasForeignKey(t => t.CustomerId);

                builder.Property(p => p.Date).HasColumnName("Date");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
