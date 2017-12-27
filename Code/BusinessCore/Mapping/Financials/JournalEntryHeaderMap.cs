using BusinessCore.Domain.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Financials
{
    public partial class JournalEntryHeaderMap : BaseEntityMap<JournalEntryHeader>
    {
        public override void Configure(EntityTypeBuilder<JournalEntryHeader> builder)
        {
            builder.ToTable("JournalEntryHeader");

            TableColumns = () =>
            {
                builder.Property(p => p.GeneralLedgerHeaderId).HasColumnName("GeneralLedgerHeaderId");
                builder.HasOne(t => t.GeneralLedgerHeader).WithMany().HasForeignKey(t => t.GeneralLedgerHeaderId);

                builder.Property(p => p.PartyId).HasColumnName("PartyId");
                builder.HasOne(t => t.Party).WithMany().HasForeignKey(t => t.PartyId);

                builder.Property(p => p.VoucherType).HasColumnName("VoucherType");
                builder.Property(p => p.Date).HasColumnName("Date");
                builder.Property(p => p.Memo).HasColumnName("Memo");
                builder.Property(p => p.ReferenceNo).HasColumnName("ReferenceNo");
                builder.Property(p => p.Posted).HasColumnName("Posted");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }

    }
}
