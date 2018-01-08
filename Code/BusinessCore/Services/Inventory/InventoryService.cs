using BusinessCore.Data;
using BusinessCore.Domain;
using BusinessCore.Domain.Items;
using System;
using System.Linq;
using System.Collections.Generic;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.TaxSystem;
using BusinessCore.Services.Security;
using BusinessCore.Security;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BusinessCore.Services.Inventory
{
    public partial class InventoryService : BaseService, IInventoryService
    {
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<InventoryControlJournal> _icjRepo;
        private readonly IRepository<Measurement> _measurementRepo;
        private readonly IRepository<ItemCategory> _itemCategoryRepo;
        private readonly IRepository<SequenceNumber> _sequenceNumberRepo;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<ItemTaxGroup> _itemTaxGroup;
        private readonly IRepository<ItemModel> _modelRepo;
        private readonly IRepository<ItemBrand> _brandRepo;


        private Expression<Func<Item, object>>[] includePropertiesOfItem =
            {
                p => p.Brand,
                pt => pt.ItemCategory,
                pc => pc.ItemTaxGroup,
                tg => tg.Model,
                tg => tg.SmallestMeasurement,
                tg => tg.SellMeasurement,
                tg => tg.PurchaseMeasurement,
            };


        public InventoryService(IRepository<Item> itemRepo,
            IRepository<Measurement> measurementRepo,
            IRepository<InventoryControlJournal> icjRepo,
            IRepository<ItemCategory> itemCategoryRepo,
            IRepository<SequenceNumber> sequenceNumberRepo,
            IRepository<Bank> bankRepo,
            IRepository<Account> accountRepo,
            IRepository<ItemTaxGroup> itemTaxGroup,
            IRepository<ItemModel> modelRepo,
             IRepository<ItemBrand> brandRepo
           )
            : base(sequenceNumberRepo, null, null, bankRepo)
        {
            _itemRepo = itemRepo;
            _measurementRepo = measurementRepo;
            _icjRepo = icjRepo;
            _itemCategoryRepo = itemCategoryRepo;
            _sequenceNumberRepo = sequenceNumberRepo;
            _bankRepo = bankRepo;
            _accountRepo = accountRepo;
            _itemTaxGroup = itemTaxGroup;
            _modelRepo = modelRepo;
            _brandRepo = brandRepo;
        }

        public InventoryControlJournal CreateInventoryControlJournal(int itemId, int? measurementId, DocumentTypes documentType, decimal? inQty, decimal? outQty, decimal? totalCost, decimal? totalAmount)
        {
            if (!inQty.HasValue && !outQty.HasValue)
                throw new MissingFieldException();

            var icj = new InventoryControlJournal()
            {
                ItemId = itemId,
                MeasurementId = measurementId,
                DocumentType = documentType,
                Date = DateTime.Now,
                INQty = inQty,
                OUTQty = outQty,
                TotalCost = totalCost,
                TotalAmount = totalAmount,
            };
            return icj;
        }
        public IQueryable<InventoryControlJournal> GetInventoryControlJournals()
        {
            var query = from f in _icjRepo.Table
                        select f;
            return query;
        }

        #region Manage Items
        public void SaveItem(Item objectToSave)
        {
            var item = GetItemById(objectToSave.Id);
            if (item != null)
            {
                item.Code = objectToSave.Code;
                item.Description = objectToSave.Description;
                item.PurchaseDescription = objectToSave.PurchaseDescription;
                item.SellDescription = objectToSave.SellDescription;
                item.Price = objectToSave.Price;
                item.Cost = objectToSave.Cost;
                item.ItemCategoryId = objectToSave.ItemCategoryId;
                item.BrandId = objectToSave.BrandId;
                item.ModelId = objectToSave.ModelId;
                item.ItemTaxGroupId = objectToSave.ItemTaxGroupId;
                item.SmallestMeasurementId = objectToSave.SmallestMeasurementId;
                item.SellMeasurementId = objectToSave.SellMeasurementId;
                item.PurchaseMeasurementId = objectToSave.PurchaseMeasurementId;

                item.PreferredVendorId = objectToSave.PreferredVendorId;

                item.InventoryAccountId = objectToSave.InventoryAccountId;
                item.SalesAccountId = objectToSave.SalesAccountId;
                item.CostOfGoodsSoldAccountId = objectToSave.CostOfGoodsSoldAccountId;
                item.InventoryAdjustmentAccountId = objectToSave.InventoryAdjustmentAccountId;

                _itemRepo.Update(item);
                objectToSave = item;
            }
            else
            {
                objectToSave.No = GetNextNumber(SequenceNumberTypes.Item).ToString();

                var sales = _accountRepo.Table.Where(a => a.AccountCode == AccountCodes.Sales_40100).FirstOrDefault();
                var inventory = _accountRepo.Table.Where(a => a.AccountCode == AccountCodes.Inventory_10800).FirstOrDefault();
                var invAdjusment = _accountRepo.Table.Where(a => a.AccountCode == AccountCodes.PurchasePriceVariance_50500).FirstOrDefault();
                var cogs = _accountRepo.Table.Where(a => a.AccountCode == AccountCodes.CostOfGoodsSold_50300).FirstOrDefault();
                var assemblyCost = _accountRepo.Table.Where(a => a.AccountCode == AccountCodes.AssemblyCost_10900).FirstOrDefault();
                var taxGroup = _itemTaxGroup.Table.Where(m => m.Name == "Regular").FirstOrDefault();

                objectToSave.SalesAccountId = sales?.Id;
                objectToSave.InventoryAccountId = inventory?.Id;
                objectToSave.CostOfGoodsSoldAccountId = cogs?.Id;
                objectToSave.InventoryAdjustmentAccountId = invAdjusment?.Id;
                objectToSave.ItemTaxGroupId = taxGroup?.Id;


                _itemRepo.Insert(objectToSave);
            }
        }

        public void DeleteItem(int itemId)
        {
            var dbObject = GetItemById(itemId);
            if (dbObject != null)
            {
                dbObject.Deleted = true;
                _itemRepo.Update(dbObject);
            }
        }

        public IQueryable<Item> GetAllItems()
        {

            var query = from item in _itemRepo.Table
                        select item;
            return query;
        }

        public Item GetItemById(int id)
        {
            var query = from item in _itemRepo.Table
                        where item.Id == id
                        select item;
            return query.FirstOrDefault();
        }

        public Item GetItemDetailById(int id)
        {
            return _itemRepo.Table
                        .Include(p => p.ItemCategory)
                        .Include(p => p.Model)
                        .Include(p => p.Brand)
                        .Include(p => p.ItemTaxGroup)
                        .Include(p => p.SmallestMeasurement)
                        .Include(p => p.SellMeasurement)
                        .Include(p => p.PurchaseMeasurement)
                        .Include(p => p.PreferredVendor)

                        .Include(p => p.InventoryAccount)
                        .Include(p => p.SalesAccount)
                        .Include(p => p.CostOfGoodsSoldAccount)
                        .Include(p => p.InventoryAdjustmentAccount)
                        .Where(item => item.Id == id)
                        .FirstOrDefault();
        }
        #endregion

        #region Manage Measurements
        public IEnumerable<Measurement> GetMeasurements()
        {
            var query = from f in _measurementRepo.Table
                        select f;
            return query.AsEnumerable();
        }
        public Measurement GetMeasurementById(int id)
        {
            return _measurementRepo.GetById(id);
        }
        #endregion

        public IQueryable<ItemCategory> GetItemCategories()
        {
            var query = from f in _itemCategoryRepo.Table
                        select f;
            return query;
        }

        #region Manage Models
        public Item GetItemByNo(string itemNo)
        {
            var query = from item in _itemRepo.Table
                        where item.Code == itemNo
                        select item;
            return query.FirstOrDefault();
        }
        public IQueryable<ItemModel> GetModels()
        {
            var query = from m in _modelRepo.Table
                        select m;
            return query;

        }
        public ItemModel GetModel(int? id)
        {
            var query = from m in _modelRepo.Table
                        where m.Id == id
                        select m;
            return query.FirstOrDefault();
        }
        public ItemModel SaveModel(ItemModel model)
        {
            if (model.Id <= 0)
                _modelRepo.Insert(model);
            else
                _modelRepo.Update(model);
            return model;
        }
        public ItemModel ModelSetActive(int id, bool isInactive)
        {
            var o = (from m in _modelRepo.Table
                     where m.Id == id
                     select m).FirstOrDefault();
            if (o != null)
            {
                o.IsActive = isInactive;
                _modelRepo.Update(o);
            }
            return o;
        }
        #endregion

        #region Manage Brands
        public IQueryable<ItemBrand> GetBrands()
        {
            var query = from m in _brandRepo.Table
                        select m;
            return query;

        }
        public ItemBrand GetBrand(int? id)
        {
            var query = from m in _brandRepo.Table
                        where m.Id == id
                        select m;
            return query.FirstOrDefault();
        }
        public ItemBrand SaveBrand(ItemBrand brand)
        {
            if (brand.Id <= 0)
                _brandRepo.Insert(brand);
            else
                _brandRepo.Update(brand);
            return brand;
        }
        public ItemBrand BrandSetActive(int id, bool isInactive)
        {
            var o = (from m in _brandRepo.Table
                     where m.Id == id
                     select m).FirstOrDefault();
            if (o != null)
            {
                o.IsActive = isInactive;
                _brandRepo.Update(o);
            }
            return o;
        }
        #endregion
    }
}
