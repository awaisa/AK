namespace WebApiCore.Models.Common
{
    public class TaxModel
    {
        public int Id { get; set; }
        public string TaxName { get; set; }
        public string TaxCode { get; set; }
        public decimal Rate { get; set; }
    }
}
