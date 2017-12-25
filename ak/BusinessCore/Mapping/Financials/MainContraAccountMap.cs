using BusinessCore.Domain.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Financials
{
    public class MainContraAccountMap : BaseEntityMap<MainContraAccount>
    {
        public override void Configure(EntityTypeBuilder<MainContraAccount> builder)
        {
            builder.ToTable("MainContraAccount");

            TableColumns = () =>
            {
                builder.Property(p => p.MainAccountId).HasColumnName("MainAccountId");
                builder.HasOne(t => t.MainAccount).WithMany().HasForeignKey(t => t.MainAccountId);
                builder.Property(p => p.RelatedContraAccountId).HasColumnName("RelatedContraAccountId");
                builder.HasOne(t => t.RelatedContraAccount).WithMany().HasForeignKey(t => t.RelatedContraAccountId);
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
