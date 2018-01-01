using System;
using System.Collections.Generic;

namespace BusinessCore.Domain.Sales
{
    public partial class SalesOrderHeader : BaseEntity, ICompanyBaseEntity
    {
        public SalesOrderHeader()
        {
            SalesOrderLines = new HashSet<SalesOrderLine>();
        }

        public int? CustomerId { get; set; }
        public int? PaymentTermId { get; set; }
        public string No { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime Date { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }

        public virtual ICollection<SalesOrderLine> SalesOrderLines { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
