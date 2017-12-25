using BusinessCore.Domain;
using WebApiCore.Models.Common;

namespace WebApiCore.Models.Inventory
{
    public class ItemCategoryModel : BaseModel
    {
        public string Name { get; set; }
        public ItemTypes ItemType { get; set; }
        public MeasurementModel Measurement { get; set; }

        public AccountModel InventoryAccount { get; set; }
        public AccountModel SalesAccount { get; set; }
        public AccountModel CostOfGoodsSoldAccount { get; set; }
        public AccountModel AdjustmentAccount { get; set; }
        public AccountModel AssemblyAccount { get; set; }
    }
}
