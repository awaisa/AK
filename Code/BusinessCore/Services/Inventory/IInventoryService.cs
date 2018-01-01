using BusinessCore.Domain;
using BusinessCore.Domain.Items;
using System.Collections.Generic;
using System.Linq;

namespace BusinessCore.Services.Inventory
{
    public partial interface IInventoryService
    {
        InventoryControlJournal CreateInventoryControlJournal(int itemId,
            int? measurementId,
            DocumentTypes documentType,
            decimal? inQty,
            decimal? outQty,
            decimal? totalCost,
            decimal? totalAmount);

        void SaveItem(Item item);
        void DeleteItem(int itemId);
        Item GetItemById(int id);
        Item GetItemDetailById(int id);
        Item GetItemByNo(string itemNo);
        IQueryable<Item> GetAllItems();

        IEnumerable<Measurement> GetMeasurements();
        Measurement GetMeasurementById(int id);

        IQueryable<ItemCategory> GetItemCategories();
        IQueryable<InventoryControlJournal> GetInventoryControlJournals();

        IQueryable<ItemModel> GetModels();
        ItemModel GetModel(int? id);
        ItemModel SaveModel(ItemModel model);
        ItemModel ModelSetActive(int id, bool isInactive);

        IQueryable<ItemBrand> GetBrands();
        ItemBrand GetBrand(int? id);
        ItemBrand SaveBrand(ItemBrand brand);
        ItemBrand BrandSetActive(int id, bool isInactive);
    }
}
