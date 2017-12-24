using BusinessCore.Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Items
{
    public partial class ItemCategoryMap : BaseEntityMap<ItemCategory>
    {
        public override void Configure(EntityTypeBuilder<ItemCategory> builder)
        {
            builder.ToTable("ItemCategory");

            TableColumns = () =>
            {
                builder.Property(p => p.ItemType).HasColumnName("ItemType");
                builder.Property(p => p.MeasurementId).HasColumnName("MeasurementId");
                builder.HasOne(t => t.Measurement).WithMany().HasForeignKey(t => t.MeasurementId);

                builder.Property(p => p.SalesAccountId).HasColumnName("SalesAccountId");
                builder.HasOne(t => t.SalesAccount).WithMany().HasForeignKey(t => t.SalesAccountId);

                builder.Property(p => p.InventoryAccountId).HasColumnName("InventoryAccountId");
                builder.HasOne(t => t.InventoryAccount).WithMany().HasForeignKey(t => t.InventoryAccountId);

                builder.Property(p => p.CostOfGoodsSoldAccountId).HasColumnName("CostOfGoodsSoldAccountId");
                builder.HasOne(t => t.CostOfGoodsSoldAccount).WithMany().HasForeignKey(t => t.CostOfGoodsSoldAccountId);

                builder.Property(p => p.AdjustmentAccountId).HasColumnName("AdjustmentAccountId");
                builder.HasOne(t => t.AdjustmentAccount).WithMany().HasForeignKey(t => t.AdjustmentAccountId);

                builder.Property(p => p.AssemblyAccountId).HasColumnName("AssemblyAccountId");
                builder.HasOne(t => t.AssemblyAccount).WithMany().HasForeignKey(t => t.AssemblyAccountId);

                builder.Property(p => p.Name).HasColumnName("Name");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
