using BusinessCore.Domain.Items;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessCore.Domain.TaxSystem;

namespace BusinessCore.Domain.Sales
{
    public partial class SalesInvoiceLineTax : BaseEntity, ICompanyBaseEntity
    {
        public int SalesInvoiceLineId { get; set; }
        public int TaxId { get; set; }

        public decimal Rate { get; set; }
        public virtual SalesInvoiceLine SalesInvoiceLine { get; set; }
        public virtual Tax Tax { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
