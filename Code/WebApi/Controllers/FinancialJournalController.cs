using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCore.Infrastructure;
using WebApiCore.Models.FinancialJournal;
using WebApiCore.Models.Mappings;
using BusinessCore.Services.Financial;
using WebApiCore.Models.Common.Search.Request;

namespace WebApiCore.Controllers
{
    public class FinancialJournalController : BaseController
    {
        private ILogger<FinancialJournalController> _log;
        private IFinancialService _service;

        public FinancialJournalController(
            IFinancialService service,
            ILogger<FinancialJournalController> log)
        {
            _service = service;
            _log = log;
        }

        [HttpGet]
        [Produces(typeof(SearchModel))]
        public IActionResult Journal([FromBody]SearchRequest request)
        {
            SearchModel model = new SearchModel
            {
                Start = request.Start
            };
            var pagesize = request.Length == 0 ? 10 : request.Length;// GetPageSize();
            //var sortcolumn = GetSortColumn();
            //var sortcolumnDir = GetSortOrder();
            var searchText = request.Search?.Value; // GetSearchedText();

            var records = _service.GetJournalEntries();

            //Total Records
            model.RecordsTotal = records.Count();
            //Filter records
            if (string.IsNullOrEmpty(searchText) == false)
            {
                records = records
                    .Where(t => t.ReferenceNo.Contains(searchText)
                    || t.Memo.Contains(searchText));
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


        [HttpPost]
        [ValidateModel]
        [Produces(typeof(JournalModel))]
        public IActionResult SaveJournal([FromBody] JournalModel model)
        {
            //server side validations add in ModelState .AddModelError([field], [message])
            if (ModelState.IsValid)
            {
                ///var obj = model.ToEntity();
                //_service.SaveCustomer(obj);
                //model = obj.ToModel();
                return Ok(model);
            }
            return new BadRequestObjectResult(ModelState);
        }
    }
}