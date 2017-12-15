using System.Collections.Generic;

namespace WebApiCore.Models.Vendor
{
    public class SearchModel
    {
        public int Start { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<VendorModel> Data { get; set; }
    }
}
