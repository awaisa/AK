using BusinessCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping
{
    public partial class SequenceNumberMap : BaseEntityMap<SequenceNumber>
    {
        public override void Configure(EntityTypeBuilder<SequenceNumber> builder)
        {
            builder.ToTable("SequenceNumber");

            TableColumns = () =>
            {
                builder.Property(p => p.SequenceNumberType).HasColumnName("SequenceNumberType");
                builder.Property(p => p.Description).HasColumnName("Description");
                builder.Property(p => p.Prefix).HasColumnName("Prefix");
                builder.Property(p => p.NextNumber).HasColumnName("NextNumber");
                builder.Property(p => p.UsePrefix).HasColumnName("UsePrefix");
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
