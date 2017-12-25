using System.Collections.Generic;

namespace WebApiCore.Models.Inventory
{
    public class SearchModel
    {
        public int Start { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<SearchRowModel> Data { get; set; }
    }

    public class SearchRowModel : BaseModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string PurchaseDescription { get; set; }
        public string SellDescription { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Price { get; set; }
        public ItemCategoryModel ItemCategory { get; set; }
        public ItemBrandModel Brand { get; set; }
        public ItemModelModel Model { get; set; }

        public MeasurementModel SmallestMeasurement { get; set; }
        public MeasurementModel SellMeasurement { get; set; }
        public MeasurementModel PurchaseMeasurement { get; set; }

        public int? PreferredVendorId { get; set; }

    }
}
