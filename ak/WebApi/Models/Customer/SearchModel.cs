using System.Collections.Generic;

namespace WebApiCore.Models.Customer
{
    public class SearchModel
    {
        public int Start { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<CustomerModel> Data { get; set; }
    }
}
