using System.Collections.Generic;

namespace WebApiCore.Models.Common
{
    public class PartyModel : BaseModel
    {
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
