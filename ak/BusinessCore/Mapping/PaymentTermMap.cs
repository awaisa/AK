using BusinessCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping
{
    public partial class PaymentTermMap : BaseEntityMap<PaymentTerm>
    {
        public override void Configure(EntityTypeBuilder<PaymentTerm> builder)
        {
            builder.ToTable("PaymentTerm");

            TableColumns = () =>
            {
                builder.Property(p => p.Description).HasColumnName("Description");
                builder.Property(p => p.PaymentType).HasColumnName("PaymentType");
                builder.Property(p => p.DueAfterDays).HasColumnName("DueAfterDays");
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
