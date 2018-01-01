using BusinessCore.Domain.Financials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Purchases
{
    public partial class PurchaseReceiptHeader : BaseEntity, ICompanyBaseEntity
    {
        public PurchaseReceiptHeader()
        {
            PurchaseReceiptLines = new HashSet<PurchaseReceiptLine>();
        }

        public int VendorId { get; set; }
        public int PurchaseOrderHeaderId { get; set; }
        public int? GeneralLedgerHeaderId { get; set; }
        public DateTime Date { get; set; }
        public string No { get; set; }
        public virtual PurchaseOrderHeader PurchaseOrderHeader { get; set; }
        public virtual GeneralLedgerHeader GeneralLedgerHeader { get; set; }
        public virtual Vendor Vendor { get; set; }

        public virtual ICollection<PurchaseReceiptLine> PurchaseReceiptLines { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public decimal GetTotalTax()
        {
            decimal totalTaxAmount = 0;
            foreach (var detail in PurchaseReceiptLines)
            {
                totalTaxAmount += detail.LineTaxAmount;
            }
            return totalTaxAmount;
        }
    }
}
