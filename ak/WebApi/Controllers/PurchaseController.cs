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

        [HttpGet("")]
        public IActionResult Invoice()
        {
            SearchModel model = new SearchModel();

            model.Start = Getstart();

            var pagesize = GetPageSize();
            var sortcolumn = GetSortColumn();
            var sortcolumnDir = GetSortOrder();
            var searchText = GetSearchedText();

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
            records = OrderBy(records, sortcolumn, sortcolumnDir == "desc");
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
                    Total = t.PurchaseInvoiceLines.Sum(i=> i.Amount)
            }).ToList();

            return Json(model);
        }

        [HttpGet("Invoice/{id:int}")]
        public InvoiceModel Invoice(int? id)
        {
            var model = new InvoiceModel();
            var invoice = _service.GetPurchaseInvoiceById(id ?? 0);
            if (invoice != null)
            {
                model.Id = invoice.Id;
                model.No = invoice.No;
                model.Description = invoice.Description;
                model.Date = invoice.Date;
                model.VendorId = invoice.VendorId;
                model.VendorInvoiceNo = invoice.VendorInvoiceNo;
                model.InvoiceItems = invoice
                    .PurchaseInvoiceLines
                    .Select(i=> new InvoiceItemModel() {
                        Id = i.Id,
                        ItemId = i.ItemId,
                        Quantity = i.Quantity,
                        Cost = i.Cost,
                        Tax = i.LineTaxAmount,
                        Discount = i.Discount,
                        Amount = i.Amount
                }).ToList();
            }
            return model;
        }

        [HttpPost("Invoice")]
        [ValidateModel]
        public InvoiceModel SaveInvoice([FromBody] InvoiceModel model)
        {
            //Note: server side validations add in ModelState .AddModelError([field], [message])
            if (ModelState.IsValid)
            {
                var invoiceHeader = model.ToEntity();
                invoiceHeader.PurchaseInvoiceLines = model.InvoiceItems.Select(i => i.ToEntity()).ToList();
            }            
            return model;
        }
    }    
}