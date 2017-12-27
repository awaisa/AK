using BusinessCore.Domain.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Financials
{
    public partial class FinancialYearMap : BaseEntityMap<FinancialYear>
    {
        public override void Configure(EntityTypeBuilder<FinancialYear> builder)
        {
            builder.ToTable("FinancialYear");

            TableColumns = () =>
            {
                builder.Property(p => p.FiscalYearCode).HasColumnName("FiscalYearCode").HasMaxLength(20).IsRequired();
                builder.Property(p => p.FiscalYearName).HasColumnName("FiscalYearName").HasMaxLength(100).IsRequired();
                builder.Property(p => p.StartDate).HasColumnName("StartDate");
                builder.Property(p => p.EndDate).HasColumnName("EndDate");
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }

    }
}
