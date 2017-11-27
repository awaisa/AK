//-----------------------------------------------------------------------
// <copyright file="IInventoryService.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

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
        Item GetItemByNo(string itemNo);
        IQueryable<Item> GetAllItems();

        IEnumerable<Measurement> GetMeasurements();
        Measurement GetMeasurementById(int id);

        IQueryable<ItemCategory> GetItemCategories();
        IQueryable<InventoryControlJournal> GetInventoryControlJournals();

        IQueryable<Model> GetModels();
        Model GetModel(int? id);
        Model SaveModel(Model model);
        Model ModelSetActive(int id, bool isInactive);

        IQueryable<Brand> GetBrands();
        Brand GetBrand(int? id);
        Brand SaveBrand(Brand brand);
        Brand BrandSetActive(int id, bool isInactive);
    }
}
