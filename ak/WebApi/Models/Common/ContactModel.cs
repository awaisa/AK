namespace WebApiCore.Models.Common
{
    public class ContactModel : IPrimary
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public bool IsPrimary { get; set; }
    }

    public interface IPrimary
    {
        bool IsPrimary { get; set; }
    }
}
