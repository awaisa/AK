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
using WebApiCore.Models.Common.Search.Request;

namespace WebApiCore.Controllers
{
    public class VendorController : BaseController
    {
        private ILogger<VendorController> _log;
        private IPurchasingService _service;

        public VendorController(
            IPurchasingService service,
            ILogger<VendorController> log)
        {
            _service = service;
            _log = log;
        }

        [HttpPost]
        [Produces(typeof(SearchModel))]
        public IActionResult Vendor([FromBody]SearchRequest request)
        {
            SearchModel model = new SearchModel
            {
                Start = request.Start
            };
            var pagesize = request.Length == 0 ? 10 : request.Length;
            var searchText = request.Search?.Value;

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
                .Select(t => t.ToRowModel()).ToList();
            return Ok(model);
        }

       
        [HttpGet("{id:int}")]
        [Produces(typeof(VendorModel))]
        public IActionResult Vendor( int? id)
        {
            var o = _service.GetVendorById(id ?? 0);
            if (o == null)
            {
                return NotFound();
            }
            var model = o.ToModel();
            return Ok(model);
        }

        [HttpPost("Save")]
        [ValidateModel]
        public IActionResult Save([FromBody] VendorModel model)
        {
            //server side validations add in ModelState .AddModelError([field], [message])
            //if (ModelState.IsValid)
            //{
            //    var obj = model.ToEntity();
            //    _service.SaveVendor(obj);
            //    model = obj.ToModel();
            //    return Ok(model);
            //}
            return new BadRequestObjectResult(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteVendor(int id)
        {
            var o = _service.GetVendorById(id);
            if (o == null)
            {
                return NotFound();
            }
            _service.DeleteVendor(id);
            return Ok();
        }

        [HttpPut]
        [Produces(typeof(VendorModel))]
        public IActionResult UpdateCustomer([FromBody]VendorModel model)
        {
            //var o = _service.GetVendorById(model.Id);
            //if (o == null)
            //{
            //    return NotFound();
            //}
            //var obj = model.ToEntity();
            //_service.UpdateVendor(obj);
            return Ok();
        }
    }    
}