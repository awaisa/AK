namespace WebApiCore.Models.Common
{
    public class ContactModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public bool IsPrimary { get; set; }
    }
}
