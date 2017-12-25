using WebApiCore.Models.Common;

namespace WebApiCore.Models.Vendor
{
    public class VendorModel : BaseModel
    {
        public string No { get; set; }
        public PartyModel Party { get; set; } = new PartyModel();
        //public ContactModel PrimaryContact { get; set; }
        public int? PaymentTermId { get; set; }
        public int? TaxGroupId { get; set; }

        public int? AccountsPayableAccountId { get; set; }
        public int? PurchaseAccountId { get; set; }
        public int? PurchaseDiscountAccountId { get; set; }
    }
}
