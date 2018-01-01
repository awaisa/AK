using BusinessCore.Domain.Items;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Sales
{
    public partial class SalesDeliveryLine : BaseEntity, ICompanyBaseEntity
    {
        public int SalesDeliveryHeaderId { get; set; }
        public int? ItemId { get; set; }
        public int? MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

        public virtual SalesDeliveryHeader SalesDeliveryHeader { get; set; }
        public virtual Item Item { get; set; }
        public virtual Measurement Measurement { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public decimal GetPriceAfterTax()
        {
            decimal priceAfterTax = 0;
            return priceAfterTax;
        }
    }
}
