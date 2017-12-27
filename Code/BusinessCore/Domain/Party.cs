using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain
{
    [Table("Party")]
    public partial class Party : BaseEntity, ICompanyBaseEntity
    {
        public Party()
        {
            Contacts = new HashSet<Contact>();
        }

        public PartyTypes PartyType { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        //public bool IsActive { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
