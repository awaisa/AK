//-----------------------------------------------------------------------
// <copyright file="VendorPayment.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using BusinessCore.Domain.Financials;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Purchases
{

    [Table("VendorPayment")]
    public partial class VendorPayment : BaseEntity, ICompanyBaseEntity
    {        
        public VendorPayment()
        { }

        public int VendorId { get; set; }
        public int? PurchaseInvoiceHeaderId { get; set; }
        public int? GeneralLedgerHeaderId { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual GeneralLedgerHeader GeneralLedgerHeader { get; set; }
        public virtual PurchaseInvoiceHeader PurchaseInvoiceHeader { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
