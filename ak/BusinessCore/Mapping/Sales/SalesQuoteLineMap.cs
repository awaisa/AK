using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesQuoteLineMap : BaseEntityMap<SalesQuoteLine>
    {
        public override void Configure(EntityTypeBuilder<SalesQuoteLine> builder)
        {
            builder.ToTable("SalesQuoteLine");

            TableColumns = () =>
            {
                builder.Property(p => p.SalesQuoteHeaderId).HasColumnName("SalesQuoteHeaderId");
                builder.HasOne(t => t.SalesQuoteHeader).WithMany(t => t.SalesQuoteLines).HasForeignKey(t => t.SalesQuoteHeaderId);

                builder.Property(p => p.ItemId).HasColumnName("ItemId");
                builder.HasOne(t => t.Item).WithMany().HasForeignKey(t => t.ItemId);

                builder.Property(p => p.MeasurementId).HasColumnName("MeasurementId");
                builder.HasOne(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);

                builder.Property(p => p.Quantity).HasColumnName("Quantity");
                builder.Property(p => p.Discount).HasColumnName("Discount");
                builder.Property(p => p.Amount).HasColumnName("Amount");
            };

            base.Configure(builder);
        }
    }
}
