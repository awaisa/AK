using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Items
{
    public partial class InventoryControlJournal : BaseEntity, ICompanyBaseEntity
    {
        public int ItemId { get; set; }
        public int? MeasurementId { get; set; }
        public DocumentTypes DocumentType { get; set; }
        public decimal? INQty { get; set; }
        public decimal? OUTQty { get; set; }
        public DateTime Date { get; set; }
        public decimal? TotalCost { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool IsReverse { get; set; }

        public virtual Item Item { get; set; }
        public virtual Measurement Measurement { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
