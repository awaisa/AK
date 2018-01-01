namespace BusinessCore.Domain
{
    public partial class Company : BaseEntity
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string CompanyCode { get; set; }
    }
}
