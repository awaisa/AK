using WebApiCore.Models.Common;
using WebApiCore.Models.Vendor;

namespace WebApiCore.Models.Inventory
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string PurchaseDescription { get; set; }
        public string SellDescription { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public ItemCategoryModel ItemCategory { get; set; }
        public ItemBrandModel Brand { get; set; }
        public ItemModelModel Model { get; set; }
        public TaxGroupModel TaxGroup { get; set; }

        public MeasurementModel SmallestMeasurement { get; set; }
        public MeasurementModel SellMeasurement { get; set; }
        public MeasurementModel PurchaseMeasurement { get; set; }

        public VendorModel PreferredVendor { get; set; }

        public AccountModel InventoryAccount { get; set; }
        public AccountModel SalesAccount { get; set; }
        public AccountModel CostOfGoodsSoldAccount { get; set; }
        public AccountModel AdjustmentAccount { get; set; }
    }
}
