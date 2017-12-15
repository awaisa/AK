using System.Collections.Generic;

namespace WebApiCore.Models.Common
{
    public abstract class PartyModel
    {
        public int PartyId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public List<ContactModel> Contacts { get; set; } = new List<ContactModel>();
    }
}
