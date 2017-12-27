using BusinessCore.Domain.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Financials
{
    public partial class JournalEntryLineMap : BaseEntityMap<JournalEntryLine>
    {
        public override void Configure(EntityTypeBuilder<JournalEntryLine> builder)
        {
            builder.ToTable("JournalEntryLine");

            TableColumns = () =>
            {
                builder.Property(p => p.JournalEntryHeaderId).HasColumnName("JournalEntryHeaderId");
                builder.HasOne(t => t.JournalEntryHeader).WithMany(t => t.JournalEntryLines).HasForeignKey(t => t.JournalEntryHeaderId);
                builder.Property(p => p.AccountId).HasColumnName("AccountId");
                builder.HasOne(t => t.Account).WithMany().HasForeignKey(t => t.AccountId);
                builder.Property(p => p.DrCr).HasColumnName("DrCr");
                builder.Property(p => p.Amount).HasColumnName("Amount");
                builder.Property(p => p.Memo).HasColumnName("Memo");
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
