using System.Collections.Generic;
using WebApiCore.Models.Common;

namespace WebApiCore.Models.Financial
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
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string AccountClass { get; set; }
    }
}
