using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessCore.Services.Inventory;
using WebApiCore.Infrastructure;
using WebApiCore.Models.Inventory;
using WebApiCore.Models.Mappings;

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

        [HttpGet]
        [Produces(typeof(SearchModel))]
        public IActionResult Item()
        {
            SearchModel model = new SearchModel
            {
                Start = Getstart()
            };
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
                .Select(t => t.ToRowModel())
                .ToList();

            return Ok(model);
        }

        [HttpGet("{id:int}")]
        [Produces(typeof(ItemModel))]
        public IActionResult Item(int? id)
        {
            var item = _service.GetItemDetailById(id ?? 0);
            if (item != null)
            {
                var model = item.ToModel();
                return Ok(model);
            }
            return NotFound(id);
        }

        [HttpPost]
        [ValidateModel]
        [Produces(typeof(ItemModel))]
        public IActionResult SaveItem([FromBody] ItemModel model)
        {
            if (ModelState.IsValid)
            {
                var item = model.ToEntity();
                _service.SaveItem(item);
                item = _service.GetItemDetailById(item.Id);

                model = item.ToModel();
                return Ok(model);
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteItem(int id)
        {
            var item = _service.GetItemDetailById(id);
            if (item == null)
            {
                return NotFound();
            }
            _service.DeleteItem(id);
            return Ok();
        }
        [HttpPut]
        [Produces(typeof(ItemModel))]
        public IActionResult UpdateItem([FromBody]ItemModel itemModel)
        {
            var items = _service.GetItemById(itemModel.Id);
            if (items == null)
            {
                return NotFound();
            }
            var item = itemModel.ToEntity();
            _service.SaveItem(item);
            return Ok();

        }
    }
}