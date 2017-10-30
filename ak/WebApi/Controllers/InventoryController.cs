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
using AlbumViewerAspNetCore;
using WebApiCore._Code;
using BusinessCore.Security;

namespace WebApiCore.Controllers
{
    [Authorize]
    [EnableCors("CorsPolicy")]
    public class InventoryController : BaseController
    {
        private ILogger<InventoryController> _log;
        private IInventoryService _service;
        private AppPrincipal _principal;

        public InventoryController(
            IInventoryService service,
            ILogger<InventoryController> log, AppPrincipal principal)
        {
            _service = service;
            _log = log;
            //WebApiCore.Helper.HttpContext.Configure();
            _principal = principal;
        }

        [Route("api/Inventory/Items")]
        public IActionResult Items()
        {
            TestData model = new TestData();

            model.start = Getstart();

            var pagesize = GetPageSize();
            var sortcolumn = GetSortColumn();
            var sortcolumnDir = GetSortOrder();
            var searchText = GetSearchedText();

            var records = _service.GetAllItems();

            //Total Records
            model.recordsTotal = records.Count();
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
            model.recordsFiltered = records.Count();
            records = OrderBy(records, sortcolumn, sortcolumnDir == "desc");
            model.data = records
                //.OrderBy(t => t.Code)
                .Skip(model.start)
                .Take(pagesize)
                //.ToList()
                .Select(t => new TestDataRecord()
            {
                    Id = t.Id,
                    Code = t.Code,
                    Description = t.Description,
                    Price = t.Price
            }).ToList();


            return Json(model);
            //return Json(model.data);
        }

        [HttpGet("api/Inventory/Item/{id:int}")]
        public ItemInModel Item(int? id)
        {
            var model = new ItemInModel();
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

        [HttpPost("api/Inventory/Item")]
        [ValidateModel]
        public async Task<ItemInModel> SaveItem([FromBody] ItemInModel model)
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

    public class TestData
    {
        public int start { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        //public List<string> data { get; set; }
        public List<TestDataRecord> data { get; set; }
    }

    public class TestDataRecord
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}