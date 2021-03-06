using BusinessCore.Domain.Items;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessCore.Domain.TaxSystem;

namespace BusinessCore.Domain.Sales
{
    public partial class SalesInvoiceLine : BaseEntity, ICompanyBaseEntity
    {
        public SalesInvoiceLine()
        {
            SalesReceiptLines = new HashSet<SalesReceiptLine>();
        }
        public int SalesInvoiceHeaderId { get; set; }
        public int ItemId { get; set; }
        public int MeasurementId { get; set; }
        public int? InventoryControlJournalId { get; set; }
        //public int? TaxId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; } //percentage
        public decimal Amount { get; set; }
        // decimal TaxAmount { get; set; } //Tax(s) should be included at front and total of tax in amount comes here
        public decimal DiscountAmount { get; set; }
        public virtual SalesInvoiceHeader SalesInvoiceHeader { get; set; }
        public virtual Item Item { get; set; }
        public virtual Measurement Measurement { get; set; }
        public virtual InventoryControlJournal InventoryControlJournal { get; set; }
        public virtual ICollection<SalesInvoiceLineTax> Taxes { get; set; }
        public virtual ICollection<SalesReceiptLine> SalesReceiptLines { get; set; }


        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public decimal ComputeLineTaxAmount()
        {
            decimal taxAmount = 0;
            
            return taxAmount;
        }

        public decimal GetAmountPaid()
        {
            return SalesReceiptLines.Sum(a => a.AmountPaid);
        }

        public bool IsPaid()
        {
            return Amount == SalesReceiptLines.Sum(a => a.AmountPaid);
        }
    }
}
