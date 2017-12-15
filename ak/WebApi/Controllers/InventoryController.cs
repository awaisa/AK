using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessCore.Services.Inventory;
using BusinessCore.Domain.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using WebApiCore;
using WebApiCore.Models;
using System.Threading.Tasks;
using WebApiCore.Infrastructure.ErrorHandling;
using WebApiCore.Infrastructure;
using BusinessCore.Security;
using WebApiCore.Models.Inventory;

namespace WebApiCore.Controllers
{
    public class InventoryController : BaseController
    {
        private ILogger<InventoryController> _log;
        private IInventoryService _service;

        public InventoryController(
            IInventoryService service,
            ILogger<InventoryController> log)
        {
            _service = service;
            _log = log;
        }
        //Fix for Swagger
        [HttpGet("")]
        public IActionResult Item()
        {
            SearchModel model = new SearchModel();

            model.Start = Getstart();

            var pagesize = GetPageSize();
            var sortcolumn = GetSortColumn();
            var sortcolumnDir = GetSortOrder();
            var searchText = GetSearchedText();

            var records = _service.GetAllItems();

            //Total Records
            model.RecordsTotal = records.Count();
            //Filter records
            if (string.IsNullOrEmpty(searchText) == false)
            {
                records = records
                    .Where(t => t.Description.Contains(searchText)
                    || t.PurchaseDescription.Contains(searchText)
                    || t.SellDescription.Contains(searchText)
                    || t.Code.Contains(searchText));
            }
            //filtered records count
            model.RecordsFiltered = records.Count();
            records = OrderBy(records, sortcolumn, sortcolumnDir == "desc");
            model.Data = records
                .Skip(model.Start)
                .Take(pagesize)
                .Select(t => new ItemModel()
            {
                    Id = t.Id,
                    Code = t.Code,
                    Description = t.Description,
                    Price = t.Price
            }).ToList();


            return Json(model);
            //return Json(model.data);
        }

        [HttpGet("Item/{id:int}")]
        public ItemModel Item(int? id)
        {
            var model = new ItemModel();
            var item = _service.GetItemById(id ?? 0);
            if (item != null)
            {
                model.Id = item.Id;
                model.Code = item.Code;
                model.Description = item.Description;
                model.PurchaseDescription = item.PurchaseDescription;
                model.SellDescription = item.SellDescription;
                model.Price = item.Price;
                model.Cost = item.Cost;
            }
            return model;
        }

        [HttpPost("Item")]
        [ValidateModel]
        public ItemModel SaveItem([FromBody] ItemModel model)
        {
            //server side validations add in ModelState .AddModelError([field], [message])
            if (ModelState.IsValid)
            {
                var item = new Item()
                {
                    Id = model.Id,
                    Code = model.Code,
                    Description = model.Description,
                    PurchaseDescription = model.PurchaseDescription,
                    SellDescription = model.SellDescription,
                    Price = model.Price,
                    Cost = model.Cost
                };
                _service.SaveItem(item);
            }            
            return model;
        }
    }    
}