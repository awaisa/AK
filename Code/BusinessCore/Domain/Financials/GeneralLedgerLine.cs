namespace BusinessCore.Domain.Financials
{
    public partial class GeneralLedgerLine : BaseEntity, ICompanyBaseEntity
    {
        public GeneralLedgerLine()
        {
        }

        public int GeneralLedgerHeaderId { get; set; }
        public int AccountId { get; set; }
        public DrOrCrSide DrCr { get; set; }
        public decimal Amount { get; set; }
        public virtual Account Account { get; set; }
        public virtual GeneralLedgerHeader GeneralLedgerHeader { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
