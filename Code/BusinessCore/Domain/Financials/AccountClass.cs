using System.Collections.Generic;

namespace BusinessCore.Domain.Financials
{
    public class AccountClass : BaseEntity
    {
        public AccountClass()
        {
            Accounts = new HashSet<Account>();
        }

        public string Name { get; set; }
        public string NormalBalance { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
