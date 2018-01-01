using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Financials
{
    public partial class JournalEntryLine : BaseEntity, ICompanyBaseEntity
    {
        public int JournalEntryHeaderId { get; set; }
        public int AccountId { get; set; }
        public DrOrCrSide DrCr { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; }

        public virtual JournalEntryHeader JournalEntryHeader { get; set; }
        public virtual Account Account { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
