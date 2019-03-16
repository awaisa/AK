using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCore.Infrastructure;
using WebApiCore.Models.Financial;
using WebApiCore.Models.Mappings;
using BusinessCore.Services.Financial;
using WebApiCore.Models.Common.Search.Request;

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

        [HttpPost]
        [Produces(typeof(SearchModel))]
        public IActionResult Account([FromBody]SearchRequest request)
        {
            SearchModel model = new SearchModel
            {
                Start = request.Start
            };
            var pagesize = request.Length == 0 ? 10 : request.Length;
            var searchText = request.Search?.Value;

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
            return Json(model);
        }

       
        [HttpGet("{id:int}")]
        [Produces(typeof(FinancialAccountModel))]
        public IActionResult Account( int? id)
        {
            var o = _service.GetAccount(id ?? 0);
            if (o == null)
            {
                return NotFound();
            }
            var model = o.ToModel2();
            return Ok(model);
        }

        [HttpPost("Save")]
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