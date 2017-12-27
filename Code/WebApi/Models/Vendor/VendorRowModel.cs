using WebApiCore.Models.Common;

namespace WebApiCore.Models.Vendor
{
    public class VendorRowModel : BaseModel
    {
        public string No { get; set; }
        public PartyModel Party { get; set; }
        public ContactModel PrimaryContact { get; set; }
        public PaymentTermModel PaymentTerm { get; set; }
        public TaxGroupModel TaxGroup { get; set; }
    }
}
