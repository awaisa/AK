using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesOrderHeaderMap : BaseEntityMap<SalesOrderHeader>
    {
        public override void Configure(EntityTypeBuilder<SalesOrderHeader> builder)
        {
            builder.ToTable("SalesOrderHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.CustomerId).HasColumnName("CustomerId");
                builder.HasOne(t => t.Customer).WithMany().HasForeignKey(t => t.CustomerId);

                builder.Property(p => p.PaymentTermId).HasColumnName("PaymentTermId");
                builder.HasOne(t => t.PaymentTerm).WithMany().HasForeignKey(t => t.PaymentTermId);

                builder.Property(p => p.No).HasColumnName("No");
                builder.Property(p => p.ReferenceNo).HasColumnName("ReferenceNo");
                builder.Property(p => p.Date).HasColumnName("Date");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
