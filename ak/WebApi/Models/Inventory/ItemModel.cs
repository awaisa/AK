using WebApiCore.Models.Common;

namespace WebApiCore.Models.Inventory
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string SellDescription { get; set; }
        public string PurchaseDescription { get; set; }
        public decimal? Price { get; set; }
        public decimal? Cost { get; set; }
        public TaxGroupModel TaxGroup { get; set; }
    }
}
