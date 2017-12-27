using WebApiCore.Models.Common;

namespace WebApiCore.Models.Customer
{
    public class CustomerRowModel : BaseModel
    {
        public string No { get; set; }
        public PartyModel Party { get; set; }
        public PaymentTermModel PaymentTerm { get; set; }
    }
}
