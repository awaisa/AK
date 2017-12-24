using BusinessCore.Domain.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Financials
{
    public partial class GeneralLedgerLineMap : BaseEntityMap<GeneralLedgerLine>
    {
        public override void Configure(EntityTypeBuilder<GeneralLedgerLine> builder)
        {
            builder.ToTable("GeneralLedgerLine");

            TableColumns = () =>
            {
                builder.Property(p => p.GeneralLedgerHeaderId).HasColumnName("GeneralLedgerHeaderId");
                builder.HasOne(t => t.GeneralLedgerHeader).WithMany(t=> t.GeneralLedgerLines).HasForeignKey(t => t.GeneralLedgerHeaderId);
                builder.Property(p => p.AccountId).HasColumnName("AccountId");
                builder.HasOne(t => t.Account).WithMany().HasForeignKey(t => t.AccountId);
                builder.Property(p => p.DrCr).HasColumnName("DrCr");
                builder.Property(p => p.Amount).HasColumnName("Amount");
            };

            base.Configure(builder);
        }
    }
}
