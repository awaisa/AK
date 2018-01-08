using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCore.Infrastructure;
using WebApiCore.Models.Sale;
using WebApiCore.Models.Mappings;
using BusinessCore.Services.Sales;

namespace WebApiCore.Controllers
{
    public class SaleController : BaseController
    {
        private ILogger<SaleController> _log;
        private ISalesService _service;

        public SaleController(
            ISalesService service,
            ILogger<SaleController> log)
        {
            _service = service;
            _log = log;
        }

        [HttpGet("Invoice")]
        [Produces(typeof(SearchModel))]
        public IActionResult Invoice()
        {
            SearchModel model = new SearchModel();

            model.Start = Getstart();

            var pagesize = GetPageSize();
            var sortcolumn = GetSortColumn();
            var sortcolumnDir = GetSortOrder();
            var searchText = GetSearchedText();

            var records = _service.GetSalesInvoices();

            //Total Records
            model.RecordsTotal = records.Count();
            //Filter records
            if (string.IsNullOrEmpty(searchText) == false)
            {
                records = records
                    .Where(
                    t => t.Description.Contains(searchText)
                        || t.Customer.No.Contains(searchText)
                        || t.Customer.Party.Name.Contains(searchText)
                        || t.Customer.Party.Address.Contains(searchText)
                        || t.Customer.Party.Email.Contains(searchText)
                        || t.No.Contains(searchText)
                    );
            }
            //filtered records count
            model.RecordsFiltered = records.Count();
            records = OrderBy(records, sortcolumn, sortcolumnDir == "desc");
            var data = records
                .Skip(model.Start)
                .Take(pagesize).ToList();

            model.Data = data
                            .Select(t => new SearchRowModel()
                            {
                                Id = t.Id,
                                No = t.No,
                                Date = t.Date,
                                CustomerId = t.CustomerId,
                                //CustomerName = t.Customer.Party.Name,
                                Total = t.ComputeTotalAmount(),

            
                        }).ToList();
            return Json(model);
        }

        [HttpGet("Invoice/{id:int}")]
        [Produces(typeof(InvoiceModel))]
        public IActionResult Invoice(int? id)
        {
            var invoice = _service.GetSalesInvoiceById(id ?? 0);
            if (invoice != null)
            {
                var model = invoice.ToModel();
                return Ok(model);
            }
            return NotFound(id);
        }

        [HttpPost("Invoice")]
        [ValidateModel]
        [Produces(typeof(InvoiceModel))]
        public IActionResult SaveInvoice([FromBody] InvoiceModel model)
        {
            if (ModelState.IsValid)
            {
                var o = model.ToEntity();
                _service.SaveSaleInvoice(o);
                model = o.ToModel();
                return Ok(model);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("Invoice/{id:int}")]
        public IActionResult DeleteInvoice(int id)
        {
            var invoice = _service.GetSalesInvoiceById(id);
            if (invoice == null)
            {
                return NotFound();
            }
            _service.DeleteInvoice(id);
            return Ok();
        }
    }    
}