using BusinessCore.Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Items
{
    public partial class InventoryControlJournalMap : BaseEntityMap<InventoryControlJournal>
    {
        public override void Configure(EntityTypeBuilder<InventoryControlJournal> builder)
        {
            builder.ToTable("InventoryControlJournal");

            TableColumns = () =>
            {
                builder.Property(p => p.ItemId).HasColumnName("ItemId");
                builder.HasOne(t => t.Item).WithMany().HasForeignKey(t => t.ItemId);
                builder.Property(p => p.MeasurementId).HasColumnName("MeasurementId");
                builder.HasOne(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);
                builder.Property(p => p.DocumentType).HasColumnName("DocumentType");
                builder.Property(p => p.INQty).HasColumnName("INQty");
                builder.Property(p => p.OUTQty).HasColumnName("OUTQty");
                builder.Property(p => p.Date).HasColumnName("Date");
                builder.Property(p => p.TotalCost).HasColumnName("TotalCost");
                builder.Property(p => p.TotalAmount).HasColumnName("TotalAmount");
                builder.Property(p => p.IsReverse).HasColumnName("IsReverse");
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }

    }
}
