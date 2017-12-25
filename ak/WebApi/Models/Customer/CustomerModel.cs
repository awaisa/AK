using WebApiCore.Models.Common;

namespace WebApiCore.Models.Customer
{
    public class CustomerModel : BaseModel
    {
        public string No { get; set; }
        public PartyModel Party { get; set; } = new PartyModel();
        public int? PaymentTermId { get; set; }
        public int? TaxGroupId { get; set; }

        public int? AccountsReceivableAccountId { get; set; }
        public int? SalesAccountId { get; set; }
        public int? SalesDiscountAccountId { get; set; }
        public int? PromptPaymentDiscountAccountId { get; set; }
        public int? CustomerAdvancesAccountId { get; set; }
    }
}
