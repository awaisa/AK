using BusinessCore.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BusinessCore.Domain.Purchases
{
    [Table("PurchaseOrderLine")]
    public partial class PurchaseOrderLine : BaseEntity, ICompanyBaseEntity
    {
        public PurchaseOrderLine()
        {
            PurchaseReceiptLines = new HashSet<PurchaseReceiptLine>();
        }

        public int PurchaseOrderHeaderId { get; set; }
        public int ItemId { get; set; }
        public int? MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public virtual PurchaseOrderHeader PurchaseOrderHeader { get; set; }
        public virtual Item Item { get; set; }
        public virtual Measurement Measurement { get; set; }

        public virtual ICollection<PurchaseReceiptLine> PurchaseReceiptLines { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public decimal? GetReceivedQuantity()
        {
            decimal? qty = 0;
            foreach (var stock in PurchaseReceiptLines)
            {
                qty += stock.InventoryControlJournal.INQty;
            }
            return qty;
        }

        public bool IsCompleted()
        {
            bool completed = false;
            decimal totalReceiptAmount = PurchaseReceiptLines.Sum(d => d.Amount);
            if (totalReceiptAmount == Amount)
                completed = true;
            return completed;
        }
    }
}
