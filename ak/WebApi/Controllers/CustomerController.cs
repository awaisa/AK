using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCore.Infrastructure;
using WebApiCore.Models.Mappings;
using WebApiCore.Models.Customer;
using BusinessCore.Services.Sales;

namespace WebApiCore.Controllers
{
    public class CustomerController : BaseController
    {
        private ILogger<CustomerController> _log;
        private ISalesService _service;

        public CustomerController(
            ISalesService service,
            ILogger<CustomerController> log)
        {
            _service = service;
            _log = log;
        }

        [HttpGet]
        public IActionResult Customer()
        {
            SearchModel model = new SearchModel
            {
                Start = Getstart()
            };

            var pagesize = GetPageSize();
            var sortcolumn = GetSortColumn();
            var sortcolumnDir = GetSortOrder();
            var searchText = GetSearchedText();

            var records = _service.GetCustomers();

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
                .Select(t => t.ToRowModel()).ToList();
            return Json(model);
        }

        [HttpGet("{id:int}")]
        public IActionResult Customer(int? id)
        {
            var o = _service.GetCustomerById(id ?? 0);
            if (o == null)
            {
                return NotFound();
            }
            var model = o.ToModel();
            return Ok(model);
        }

        [HttpPost]
        [ValidateModel]
        public IActionResult SaveCustomer([FromBody] CustomerModel model)
        {
            //server side validations add in ModelState .AddModelError([field], [message])
            if (ModelState.IsValid)
            {
                var obj = model.ToEntity();
                _service.SaveCustomer(obj);
                model = obj.ToModel();
                return Ok(model);
            }
            return BadRequest(ModelState);
        }
    }    
}