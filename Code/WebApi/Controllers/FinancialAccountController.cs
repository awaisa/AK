using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCore.Infrastructure;
using WebApiCore.Models.Financial;
using WebApiCore.Models.Mappings;
using BusinessCore.Services.Financial;

namespace WebApiCore.Controllers
{
    public class FinancialAccountController : BaseController
    {
        private ILogger<FinancialAccountController> _log;
        private IFinancialService _service;

        public FinancialAccountController(
            IFinancialService service,
            ILogger<FinancialAccountController> log)
        {
            _service = service;
            _log = log;
        }

        [HttpGet]
        public IActionResult Account()
        {
            SearchModel model = new SearchModel
            {
                Start = Getstart()
            };

            var pagesize = GetPageSize();
            var sortcolumn = GetSortColumn();
            var sortcolumnDir = GetSortOrder();
            var searchText = GetSearchedText();

            var records = _service.GetAccounts();

            //Total Records
            model.RecordsTotal = records.Count();
            //Filter records
            if (string.IsNullOrEmpty(searchText) == false)
            {
                records = records
                    .Where(t => t.AccountCode.Contains(searchText)
                    || t.AccountName.Contains(searchText)
                    || t.AccountClass.Name.Contains(searchText)
                    );
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
        [Produces(typeof(FinancialAccountModel))]
        public IActionResult Account( int? id)
        {
            var o = _service.GetVendorById(id ?? 0);
            if (o == null)
            {
                return NotFound();
            }
            var model = o.ToModel();
            return Ok(model);
        }

        [HttpPost]
        [ValidateModel]
        public IActionResult Save([FromBody] FinancialAccountModel model)
        {
            //server side validations add in ModelState .AddModelError([field], [message])
            if (ModelState.IsValid)
            {
                //var obj = model.ToEntity();
                //_service.SaveVendor(obj);
                //model = obj.ToModel();
                return Ok(model);
            }
            return new BadRequestObjectResult(ModelState);
        }
    }    
}