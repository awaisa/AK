using System.Collections.Generic;
using WebApiCore.Models.Common;

namespace WebApiCore.Models.Customer
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
        public PartyModel Party { get; set; } = new PartyModel();
        public PaymentTermModel PaymentTerm { get; set; }
        public TaxGroupModel TaxGroup { get; set; }
    }
}
