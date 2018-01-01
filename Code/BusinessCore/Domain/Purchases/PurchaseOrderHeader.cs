using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Purchases
{
    public partial class PurchaseOrderHeader : BaseEntity, ICompanyBaseEntity
    {
        public PurchaseOrderHeader()
        {
            PurchaseOrderLines = new HashSet<PurchaseOrderLine>();
            PurchaseReceipts = new HashSet<PurchaseReceiptHeader>();
        }
        public int? VendorId { get; set; }
        public int? PurchaseInvoiceHeaderId { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual PurchaseInvoiceHeader PurchaseInvoiceHeader { get; set; }

        public virtual ICollection<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public virtual ICollection<PurchaseReceiptHeader> PurchaseReceipts { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public bool IsCompleted()
        {
            foreach (var line in PurchaseOrderLines)
            {
                foreach (var receipt in PurchaseReceipts)
                {
                    var totalReceivedQuatity = receipt.PurchaseReceiptLines.Where(l => l.PurchaseOrderLineId == line.Id).Sum(q => q.ReceivedQuantity);

                    if (totalReceivedQuatity >= line.Quantity)
                        return true;
                }
            }

            return false;
        }

        public bool IsPaid()
        {
            bool paid = false;
            //decimal totalPaidAmount = Payments.Where(p => p.PurchaseOrderId == Id).Sum(a => a.Amount);
            //decimal totalPurchaseAmount = PurchaseOrderLines.Sum(d => d.Amount);
            //if (totalPaidAmount == totalPurchaseAmount)
            //    paid = true;
            return paid;
        }
    }
}
