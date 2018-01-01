using System;
using System.Collections.Generic;

namespace BusinessCore.Domain.Sales
{
    public partial class SalesQuoteHeader : BaseEntity, ICompanyBaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<SalesQuoteLine> SalesQuoteLines { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public SalesQuoteHeader()
        {
            SalesQuoteLines = new HashSet<SalesQuoteLine>();
        }
    }
}
