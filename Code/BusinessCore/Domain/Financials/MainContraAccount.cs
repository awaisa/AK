namespace BusinessCore.Domain.Financials
{
    public class MainContraAccount : BaseEntity, ICompanyBaseEntity
    {
        public int? MainAccountId { get; set; }
        public int? RelatedContraAccountId { get; set; }
        public virtual Account MainAccount { get; set; }
        public virtual Account RelatedContraAccount { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
