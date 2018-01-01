namespace BusinessCore.Domain
{
    public partial class SequenceNumber : BaseEntity, ICompanyBaseEntity
    {
        public SequenceNumberTypes SequenceNumberType { get; set; }
        public string Description { get; set; }
        public string Prefix { get; set; }
        public int NextNumber { get; set; }
        public bool UsePrefix { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
