//-----------------------------------------------------------------------
// <copyright file="PurchaseInvoiceHeader.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using BusinessCore.Domain.Financials;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Purchases
{
    [Table("PurchaseInvoiceHeader")]
    public partial class PurchaseInvoiceHeader : BaseEntity, ICompanyBaseEntity
    {
        public PurchaseInvoiceHeader()
        {
            PurchaseInvoiceLines = new HashSet<PurchaseInvoiceLine>();
            PurchaseOrders = new HashSet<PurchaseOrderHeader>();
            VendorPayments = new HashSet<VendorPayment>();
        }
        
        public int? VendorId { get; set; }
        public int? GeneralLedgerHeaderId { get; set; }
        public DateTime Date { get; set; }
        public string No { get; set; }
        [Required]
        public string VendorInvoiceNo { get; set; }
        public string Description { get; set; }

        public virtual Vendor Vendor { get; set; }
        public virtual GeneralLedgerHeader GeneralLedgerHeader { get; set; }

        public virtual ICollection<PurchaseInvoiceLine> PurchaseInvoiceLines { get; set; }
        public virtual ICollection<PurchaseOrderHeader> PurchaseOrders { get; set; }
        public virtual ICollection<VendorPayment> VendorPayments { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public decimal GetTotalTax()
        {
            decimal totalTaxAmount = 0;
            foreach (var detail in PurchaseInvoiceLines)
            {
                totalTaxAmount += detail.LineTaxAmount;
            }
            return totalTaxAmount;
        }

        public bool IsPaid()
        {
            return this.GeneralLedgerHeader.GeneralLedgerLines.Where(dr => dr.DrCr == DrOrCrSide.Dr).Sum(l => l.Amount) == VendorPayments.Sum(a => a.Amount);
        }
    }
}
