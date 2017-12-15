using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessCore.Domain.Items;
using WebApiCore.Infrastructure;
using BusinessCore.Services.Purchasing;
using WebApiCore.Models.Vendor;
using BusinessCore.Domain.Purchases;
using WebApiCore.Models.Mappings;

namespace WebApiCore.Controllers
{
    public class VendorController : BaseController
    {
        private ILogger<InventoryController> _log;
        private IPurchasingService _service;

        public VendorController(
            IPurchasingService service,
            ILogger<InventoryController> log)
        {
            _service = service;
            _log = log;
        }

        [HttpGet("")]
        public IActionResult Vendor()
        {
            SearchModel model = new SearchModel
            {
                Start = Getstart()
            };

            var pagesize = GetPageSize();
            var sortcolumn = GetSortColumn();
            var sortcolumnDir = GetSortOrder();
            var searchText = GetSearchedText();

            var records = _service.GetVendors();

            //Total Records
            model.RecordsTotal = records.Count();
            //Filter records
            if (string.IsNullOrEmpty(searchText) == false)
            {
                records = records
                    .Where(t => t.No.Contains(searchText)
                    || t.Party.Name.Contains(searchText)
                    || t.Party.Email.Contains(searchText)
                    || t.Party.Address.Contains(searchText)
                    || t.Party.Fax.Contains(searchText)
                    || t.Party.Phone.Contains(searchText)
                    || t.Party.Website.Contains(searchText));
            }
            //filtered records count
            model.RecordsFiltered = records.Count();
            records = OrderBy(records, sortcolumn, sortcolumnDir == "desc");
            model.Data = records
                .Skip(model.Start)
                .Take(pagesize)
                .Select(t => t.ToModel()).ToList();
            return Json(model);
        }

        [HttpGet("Vendor/{id:int}")]
        public IActionResult Vendor(int? id)
        {
            var o = _service.GetVendorById(id ?? 0);
            if (o == null)
            {
                return NotFound();
            }
            var model = o.ToModel();
            return Ok(model);
        }

        [HttpPost("Vendor")]
        [ValidateModel]
        public IActionResult Save([FromBody] VendorModel model)
        {
            //server side validations add in ModelState .AddModelError([field], [message])
            if (ModelState.IsValid)
            {
                var obj = model.ToEntity();
                _service.UpdateVendor(obj);
                model = obj.ToModel();
                return Ok(model);
            }
            return BadRequest(ModelState);
        }
    }    
}