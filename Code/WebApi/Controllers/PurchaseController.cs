using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessCore.Services.Purchasing;
using BusinessCore.Domain.Items;
using Microsoft.AspNetCore.Authorization;
using WebApiCore.Infrastructure;
using WebApiCore.Models.Purchase;
using BusinessCore.Domain.Purchases;
using WebApiCore.Models.Mappings;
using WebApiCore.Models.Common.Search.Request;

namespace WebApiCore.Controllers
{
    public class PurchaseController : BaseController
    {
        private ILogger<PurchaseController> _log;
        private IPurchasingService _service;

        public PurchaseController(
            IPurchasingService service,
            ILogger<PurchaseController> log)
        {
            _service = service;
            _log = log;
        }

        [HttpPost]
        [Produces(typeof(SearchModel))]
        public IActionResult Invoice([FromBody]SearchRequest request)
        {
            SearchModel model = new SearchModel
            {
                Start = request.Start
            };
            var pagesize = request.Length == 0 ? 10 : request.Length;
            var searchText = request.Search?.Value;

            var records = _service.GetPurchaseInvoices();

            //Total Records
            model.RecordsTotal = records.Count();
            //Filter records
            if (string.IsNullOrEmpty(searchText) == false)
            {
                records = records
                    .Where(t => t.Description.Contains(searchText)
                    || t.Vendor.No.Contains(searchText));
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
                .Select(t => new InvoiceRowModel()
                {
                    Id = t.Id,
                    No = t.No,
                    Date = t.Date,
                    VendorId = t.VendorId,
                    VendorName = t.Vendor.Party.Name,
                    Total = t.PurchaseInvoiceLines.Sum(i => i.Amount)
                }).ToList();

            return Ok(model);
        }

        [HttpGet("{id:int}")]
        [Produces(typeof(InvoiceModel))]
        public IActionResult Invoice(int? id)
        {
            var invoice = _service.GetPurchaseInvoiceById(id ?? 0);
            if (invoice != null)
            {
                var model = invoice.ToModel();
                return Ok(model);
            }
            return NotFound(id);
        }

        [HttpPost("Invoice")]
        [HttpPut("Invoice")]
        [ValidateModel]
        [Produces(typeof(InvoiceModel))]
        public IActionResult SaveInvoice([FromBody] InvoiceModel model)
        {
            if (ModelState.IsValid)
            {
                var o = model.ToEntity();
                _service.SavePurchaseInvoice(o);
                model = o.ToModel();
                return Ok(model);
            }
            return BadRequest(ModelState);
        }
    }
}