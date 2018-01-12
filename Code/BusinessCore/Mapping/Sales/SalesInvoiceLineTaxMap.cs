using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesInvoiceLineTaxMap : BaseEntityMap<SalesInvoiceLineTax>
    {
        public override void Configure(EntityTypeBuilder<SalesInvoiceLineTax> builder)
        {
            builder.ToTable("SalesInvoiceLineTax");

            TableColumns = () =>
            {
                builder.Property(p => p.SalesInvoiceLineId).HasColumnName("SalesInvoiceLineId");
                builder.HasOne(t => t.SalesInvoiceLine).WithMany(t=> t.Taxes).HasForeignKey(t => t.SalesInvoiceLineId);

                builder.Property(p => p.TaxId).HasColumnName("TaxId");
                builder.HasOne(t => t.Tax).WithMany().HasForeignKey(t => t.TaxId);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
