using BusinessCore.Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Items
{
    public partial class ItemMap : BaseEntityMap<Item>
    {
        public override void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");

            TableColumns = () =>
            {
                builder.Property(p => p.ItemCategoryId).HasColumnName("ItemCategoryId");
                builder.HasOne(t => t.ItemCategory).WithMany().HasForeignKey(t => t.ItemCategoryId);

                builder.Property(p => p.SmallestMeasurementId).HasColumnName("SmallestMeasurementId");
                builder.HasOne(t => t.SmallestMeasurement).WithMany().HasForeignKey(t => t.SmallestMeasurementId);

                builder.Property(p => p.SellMeasurementId).HasColumnName("SellMeasurementId");
                builder.HasOne(t => t.SellMeasurement).WithMany().HasForeignKey(t => t.SellMeasurementId);

                builder.Property(p => p.PurchaseMeasurementId).HasColumnName("PurchaseMeasurementId");
                builder.HasOne(t => t.PurchaseMeasurement).WithMany().HasForeignKey(t => t.PurchaseMeasurementId);

                builder.Property(p => p.PreferredVendorId).HasColumnName("PreferredVendorId");
                builder.HasOne(t => t.PreferredVendor).WithMany().HasForeignKey(t => t.PreferredVendorId);

                builder.Property(p => p.ItemTaxGroupId).HasColumnName("ItemTaxGroupId");
                builder.HasOne(t => t.ItemTaxGroup).WithMany().HasForeignKey(t => t.ItemTaxGroupId);

                builder.Property(p => p.SalesAccountId).HasColumnName("SalesAccountId");
                builder.HasOne(t => t.SalesAccount).WithMany().HasForeignKey(t => t.SalesAccountId);

                builder.Property(p => p.InventoryAccountId).HasColumnName("InventoryAccountId");
                builder.HasOne(t => t.InventoryAccount).WithMany().HasForeignKey(t => t.InventoryAccountId);

                builder.Property(p => p.CostOfGoodsSoldAccountId).HasColumnName("CostOfGoodsSoldAccountId");
                builder.HasOne(t => t.CostOfGoodsSoldAccount).WithMany().HasForeignKey(t => t.CostOfGoodsSoldAccountId);

                builder.Property(p => p.InventoryAdjustmentAccountId).HasColumnName("InventoryAdjustmentAccountId");
                builder.HasOne(t => t.InventoryAdjustmentAccount).WithMany().HasForeignKey(t => t.InventoryAdjustmentAccountId);

                builder.Property(p => p.No).HasColumnName("No");
                builder.Property(p => p.Code).HasColumnName("Code");
                builder.Property(p => p.Description).HasColumnName("Description");
                builder.Property(p => p.PurchaseDescription).HasColumnName("PurchaseDescription");
                builder.Property(p => p.SellDescription).HasColumnName("SellDescription");
                builder.Property(p => p.Cost).HasColumnName("Cost");
                builder.Property(p => p.Price).HasColumnName("Price");

                builder.Property(p => p.BrandId).HasColumnName("BrandId");
                builder.HasOne(t => t.Brand).WithMany().HasForeignKey(t => t.BrandId);

                builder.Property(p => p.ModelId).HasColumnName("ModelId");
                builder.HasOne(t => t.Model).WithMany().HasForeignKey(t => t.ModelId);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
