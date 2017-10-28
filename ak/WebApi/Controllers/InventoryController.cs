using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessCore.Services.Inventory;
using BusinessCore.Domain.Items;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using WebApiCore;

namespace WebApiCore.Controllers
{
    [Authorize]
    [EnableCors("CorsPolicy")]
    public class InventoryController : BaseController
    {
        private ILogger<InventoryController> _log;
        private IInventoryService _inventoryService;

        public InventoryController(
            IInventoryService inventoryService,
            ILogger<InventoryController> log)
        {
            _inventoryService = inventoryService;
            _log = log;
            //WebApiCore.Helper.HttpContext.Configure();
        }

        [Route("api/Inventory/GetItems")]
        public IActionResult GetAlbums(/*int page = -1, int pageSize = 15*/)
        {
            //_log.LogInformation("api/Inventory/GetItems?page={0}&pageSize={1}", page, pageSize);
            //var repo = new AlbumRepository(context);
            //return await _inventoryService.GetAllItems(page, pageSize);

            //var query = _inventoryService.GetAllItems();

            //if (page > 0)
            //{
            //    query = query
            //                    .Skip((page - 1) * pageSize)
            //                    .Take(pageSize);
            //}
            //return query.ToList();

            TestData model = new TestData();

            model.start = Getstart();

            var pagesize = GetPageSize();
            var sortcolumn = GetSortColumn();
            var sortcolumnDir = GetSortOrder();
            var searchText = GetSearchedText();

            var records = _inventoryService.GetAllItems();

            //Total Records
            model.recordsTotal = records.Count();
            //Filter records
            if (string.IsNullOrEmpty(searchText) == false)
            {
                records = records
                    .Where(t => t.Description.Contains(searchText))
                    .Where(t => t.PurchaseDescription.Contains(searchText))
                    .Where(t => t.SellDescription.Contains(searchText))
                    .Where(t => t.Code.Contains(searchText));
            }
            //filtered records count
            model.recordsFiltered = records.Count();
            records = OrderBy(records, sortcolumn, sortcolumnDir == "desc");
            //skip
            //int skip = ((model.start == 0 ? 1 : model.start - 1)) * pagesize;
            model.data = records
                //.OrderBy(t => t.Code)
                .Skip(model.start)
                .Take(pagesize)
                //.ToList()
                .Select(t => new TestDataRecord()
            {
                    Code = t.Code,
                    Description = t.Description,
                    Price = t.Price
            }).ToList();


            return Json(model);
            //return Json(model.data);
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
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}