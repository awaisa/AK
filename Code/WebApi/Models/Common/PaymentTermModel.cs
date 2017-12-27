using BusinessCore.Domain;

namespace WebApiCore.Models.Common
{
    public class PaymentTermModel : BaseModel
    {
        public string Description { get; set; }
        public PaymentTypes PaymentType { get; set; }
        public int? DueAfterDays { get; set; }
    }
}
