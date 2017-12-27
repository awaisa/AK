using WebApiCore.Models.Common;
using WebApiCore.Models.Vendor;

namespace WebApiCore.Models.Inventory
{
    public class ItemModel : BaseModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string PurchaseDescription { get; set; }
        public string SellDescription { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public int? ItemCategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? ModelId { get; set; }
        public int? TaxGroupId { get; set; }

        public int? SmallestMeasurementId { get; set; }
        public int? SellMeasurementId { get; set; }
        public int? PurchaseMeasurementId { get; set; }

        public int? PreferredVendorId { get; set; }

        public int? InventoryAccountId { get; set; }
        public int? SalesAccountId { get; set; }
        public int? CostOfGoodsSoldAccountId { get; set; }
        public int? AdjustmentAccountId { get; set; }
    }
}
