using WebApiCore.Models.Common;

namespace WebApiCore.Models.Customer
{
    public class CustomerModel : PartyModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        //public ContactModel PrimaryContact { get; set; }
        public PaymentTermModel PaymentTerm { get; set; }
        public TaxGroupModel TaxGroup { get; set; }
    }
}
