using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class SalesOrderLineMap : BaseEntityMap<SalesOrderLine>
    {
        public override void Configure(EntityTypeBuilder<SalesOrderLine> builder)
        {
            builder.ToTable("SalesOrderLine");

            TableColumns = () =>
            {
                builder.Property(p => p.SalesOrderHeaderId).HasColumnName("SalesOrderHeaderId");
                builder.HasOne(t => t.SalesOrderHeader).WithMany(t => t.SalesOrderLines).HasForeignKey(t => t.SalesOrderHeaderId);

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
