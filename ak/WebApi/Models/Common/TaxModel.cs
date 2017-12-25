namespace WebApiCore.Models.Common
{
    public class TaxModel : BaseModel
    {
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public decimal Rate { get; set; }
    }
}
