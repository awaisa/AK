using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Financials
{
    public partial class JournalEntryHeader : BaseEntity, ICompanyBaseEntity
    {
        public JournalEntryHeader()
        {
            JournalEntryLines = new HashSet<JournalEntryLine>();
        }

        public int? GeneralLedgerHeaderId { get; set; }
        public int? PartyId { get; set; }
        public JournalVoucherTypes? VoucherType { get; set; }
        public DateTime Date { get; set; }
        public string Memo { get; set; }
        public string ReferenceNo { get; set; }
        public bool? Posted { get; set; }
        public virtual GeneralLedgerHeader GeneralLedgerHeader { get; set; }
        public virtual Party Party { get; set; }

        public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
