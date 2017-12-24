using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Financials
{
    [Table("Bank")]
    public partial class Bank : BaseEntity, ICompanyBaseEntity
    {
        public BankTypes Type { get; set; }
        public string Name { get; set; }
        public int? AccountId { get; set; }
        public string BankName { get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public bool IsDefault { get; set; }
        //public bool IsActive { get; set; }
        public virtual Account Account { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
