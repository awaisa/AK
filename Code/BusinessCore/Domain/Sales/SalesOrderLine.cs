using BusinessCore.Domain.Items;

namespace BusinessCore.Domain.Sales
{
    public partial class SalesOrderLine : BaseEntity, ICompanyBaseEntity
    {
        public int SalesOrderHeaderId { get; set; }
        public int ItemId { get; set; }
        public int MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public virtual SalesOrderHeader SalesOrderHeader { get; set; }
        public virtual Item Item { get; set; }
        public virtual Measurement Measurement { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
