using BusinessCore.Domain.Financials;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessCore.Domain.Items;

namespace BusinessCore.Domain.Sales
{
    [Table("SalesReceiptLine")]
    public partial class SalesReceiptLine : BaseEntity, ICompanyBaseEntity
    {
        public SalesReceiptLine()
        {            
        }

        public int SalesReceiptHeaderId { get; set; }
        public int? SalesInvoiceLineId { get; set; }
        public int? ItemId { get; set; }
        public int? AccountToCreditId { get; set; }
        public int? MeasurementId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public virtual SalesReceiptHeader SalesReceiptHeader { get; set; }
        public virtual Item Item { get; set; }
        public virtual SalesInvoiceLine SalesInvoiceLine { get; set; }
        public virtual Account AccountToCredit { get; set; }
        public virtual Measurement Measurement { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
