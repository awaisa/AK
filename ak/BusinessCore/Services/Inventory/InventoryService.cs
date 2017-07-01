//-----------------------------------------------------------------------
// <copyright file="InventoryService.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using BusinessCore.Data;
using BusinessCore.Domain;
using BusinessCore.Domain.Items;
using System;
using System.Linq;
using System.Collections.Generic;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.TaxSystem;
using BusinessCore.Services.Security;

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
        private readonly IRepository<Model> _modelRepo;
        private readonly IRepository<Brand> _brandRepo;

        public InventoryService(IRepository<Item> itemRepo,
            IRepository<Measurement> measurementRepo,
            IRepository<InventoryControlJournal> icjRepo,
            IRepository<ItemCategory> itemCategoryRepo,
            IRepository<SequenceNumber> sequenceNumberRepo,
            IRepository<Bank> bankRepo,
            IRepository<Account> accountRepo,
            IRepository<ItemTaxGroup> itemTaxGroup,
            IRepository<Model> modelRepo,
             IRepository<Brand> brandRepo
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

        public void AddItem(Item item)
        {
            item.No = GetNextNumber(SequenceNumberTypes.Item).ToString();

            var sales = _accountRepo.Table.Where(a => a.AccountCode == "40100").FirstOrDefault();
            var inventory = _accountRepo.Table.Where(a => a.AccountCode == "10800").FirstOrDefault();
            var invAdjusment = _accountRepo.Table.Where(a => a.AccountCode == "50500").FirstOrDefault();
            var cogs = _accountRepo.Table.Where(a => a.AccountCode == "50300").FirstOrDefault();
            var assemblyCost = _accountRepo.Table.Where(a => a.AccountCode == "10900").FirstOrDefault();

            item.SalesAccount = sales;
            item.InventoryAccount = inventory;
            item.CostOfGoodsSoldAccount = cogs;
            item.InventoryAdjustmentAccount = invAdjusment;

            item.ItemTaxGroup = _itemTaxGroup.Table.Where(m => m.Name == "Regular").FirstOrDefault();

            _itemRepo.Insert(item);
        }

        public void UpdateItem(Item item)
        {
            _itemRepo.Update(item);
        }

        public void DeleteItem(int itemId)
        {
            throw new NotImplementedException();
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

        public IQueryable<ItemCategory> GetItemCategories()
        {
            var query = from f in _itemCategoryRepo.Table
                        select f;
            return query;
        }

        public IQueryable<InventoryControlJournal> GetInventoryControlJournals()
        {
            var query = from f in _icjRepo.Table
                        select f;
            return query;
        }

        public Item GetItemByNo(string itemNo)
        {
            var query = from item in _itemRepo.Table
                        where item.Code == itemNo
                        select item;
            return query.FirstOrDefault();
        }


        public IQueryable<Model> GetModels()
        {
            var query = from m in _modelRepo.Table
                        select m;
            return query;

        }

        public Model GetModel(int? id)
        {
            var query = from m in _modelRepo.Table
                        where m.Id == id
                        select m;
            return query.FirstOrDefault();
        }

        public Model SaveModel(Model model)
        {
            if (model.Id <= 0)
                _modelRepo.Insert(model);
            else
                _modelRepo.Update(model);
            return model;
        }

        public IQueryable<Brand> GetBrands()
        {
            var query = from m in _brandRepo.Table
                        select m;
            return query;

        }

        public Brand GetBrand(int? id)
        {
            var query = from m in _brandRepo.Table
                        where m.Id == id
                        select m;
            return query.FirstOrDefault();
        }

        public Brand SaveBrand(Brand brand)
        {
            if (brand.Id <= 0)
                _brandRepo.Insert(brand);
            else
                _brandRepo.Update(brand);
            return brand;
        }

        public Model ModelSetActive(int id, bool isInactive)
        {
            var o = (from m in _modelRepo.Table
                     where m.Id == id
                     select m).FirstOrDefault();
            if (o != null)
            {
                o.IsInactive = isInactive;
                _modelRepo.Update(o);
            }
            return o;
        }

        public Brand BrandSetActive(int id, bool isInactive)
        {
            var o = (from m in _brandRepo.Table
                     where m.Id == id
                     select m).FirstOrDefault();
            if (o != null)
            {
                o.IsInactive = isInactive;
                _brandRepo.Update(o);
            }
            return o;
        }
    }
}
