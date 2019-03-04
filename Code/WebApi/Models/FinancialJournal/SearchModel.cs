using System;
using System.Collections.Generic;
using WebApiCore.Models.Common;

namespace WebApiCore.Models.FinancialJournal
{
    public class SearchModel
    {
        public int Start { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<SearchRowModel> Data { get; set; }
    }

    public class SearchRowModel : BaseModel
    {
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; }
        public decimal Amount { get; set; }
        public string VoucherType { get; set; }
    }
}
