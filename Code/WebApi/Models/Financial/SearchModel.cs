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
        public string No { get; set; }
    }
}
