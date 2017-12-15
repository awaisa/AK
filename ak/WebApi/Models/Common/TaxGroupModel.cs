using System.Collections.Generic;

namespace WebApiCore.Models.Common
{
    public class TaxGroupModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<TaxModel> Taxes { get; set; }
    }
}
