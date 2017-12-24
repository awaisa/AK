using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.TaxSystem;

namespace BusinessCore.Mapping.TaxSystem
{
    public partial class TaxMap : BaseEntityMap<Tax>
    {
        public override void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder.ToTable("Tax");

            TableColumns = () =>
            {
                builder.Property(p => p.SalesAccountId).HasColumnName("SalesAccountId");
                builder.HasOne(t => t.SalesAccount).WithMany().HasForeignKey(t => t.SalesAccountId);

                builder.Property(p => p.PurchasingAccountId).HasColumnName("PurchasingAccountId");
                builder.HasOne(t => t.PurchasingAccount).WithMany().HasForeignKey(t => t.PurchasingAccountId);

                builder.Property(p => p.TaxName).HasColumnName("TaxName").HasMaxLength(50);
                builder.Property(p => p.TaxCode).HasColumnName("TaxCode").HasMaxLength(16);
                builder.Property(p => p.Rate).HasColumnName("Rate");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
