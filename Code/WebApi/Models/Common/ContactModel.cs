using FluentValidation;

namespace WebApiCore.Models.Common
{
    public class ContactModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class ContactModelValidator : AbstractValidator<ContactModel>
    {
        public ContactModelValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty();
            RuleFor(m => m.MiddleName).NotEmpty();
            RuleFor(m => m.LastName).NotEmpty();
        }
    }
}
