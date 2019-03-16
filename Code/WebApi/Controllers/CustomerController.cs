using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCore.Infrastructure;
using WebApiCore.Models.Mappings;
using WebApiCore.Models.Customer;
using BusinessCore.Services.Sales;
using WebApiCore.Models.Common.Search.Request;

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

        [HttpPost]
        [Produces(typeof(SearchModel))]
        public IActionResult Customer([FromBody]SearchRequest request)
        {
            SearchModel model = new SearchModel
            {
                Start = request.Start
            };
            var pagesize = request.Length == 0 ? 10 : request.Length;
            var searchText = request.Search?.Value;

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
        [Produces(typeof(CustomerModel))]
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

        [HttpPost("Save")]
        [ValidateModel]
        [Produces(typeof(CustomerModel))]
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
            return new BadRequestObjectResult(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCustomer(int id)
        {
            var o = _service.GetCustomerById(id);
            if (o == null)
            {
                return NotFound();
            }
            _service.DeleteCustomer(id);
            return Ok();
        }
        //[HttpPut]
        //[Produces(typeof(CustomerModel))]
        //public IActionResult UpdateCustomer([FromBody]CustomerModel model)
        //{
        //    var o = _service.GetCustomerById(model.Id);
        //    if (o == null)
        //    {
        //        return NotFound();
        //    }
        //    var obj = model.ToEntity();
        //    _service.UpdateCustomer(obj);
        //    return Ok();
        //}
    }    
}