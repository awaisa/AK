using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Sales
{
    public partial class CustomerAllocation : BaseEntity, ICompanyBaseEntity
    {
        public CustomerAllocation()
        {
        }

        public int CustomerId { get; set; }
        public int SalesInvoiceHeaderId { get; set; }
        public int SalesReceiptHeaderId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual SalesInvoiceHeader SalesInvoiceHeader { get; set; }
        public virtual SalesReceiptHeader SalesReceiptHeader { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
