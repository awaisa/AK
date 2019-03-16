using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessCore.Services.Inventory;
using WebApiCore.Infrastructure;
using WebApiCore.Models.Inventory;
using WebApiCore.Models.Mappings;
using WebApiCore.Models.Common.Search.Request;

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

        [HttpPost]
        [Produces(typeof(SearchModel))]
        public IActionResult Item([FromBody]SearchRequest request)
        {
            SearchModel model = new SearchModel
            {
                Start = request.Start
            };
            var pagesize = request.Length == 0 ? 10 : request.Length;
            var searchText = request.Search?.Value;

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

            if (request.Order != null)
            {
                var columnIndex = request.Order[0].Column;
                var sortDirection = request.Order[0].Dir;
                var columnName = request.Columns[columnIndex].Data;
                records = OrderBy(records, columnName, sortDirection == "desc");
            }
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

        [HttpPost("Save")]
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