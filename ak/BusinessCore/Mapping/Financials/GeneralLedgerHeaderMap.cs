using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Financials
{
    public partial class GeneralLedgerHeaderMap : BaseEntityMap<GeneralLedgerHeader>
    {
        public override void Configure(EntityTypeBuilder<GeneralLedgerHeader> builder)
        {
            builder.ToTable("GeneralLedgerHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.Date).HasColumnName("Date");
                builder.Property(p => p.DocumentType).HasColumnName("DocumentType");
                builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(500);
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
