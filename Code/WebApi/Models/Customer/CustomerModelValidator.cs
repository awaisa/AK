using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using WebApiCore.Models.Common;

namespace WebApiCore.Models.Customer
{
    public class CustomerModelValidator : AbstractValidator<CustomerModel>
    {
        public CustomerModelValidator()
        {
            RuleFor(m => m.No).NotEmpty();
            RuleFor(m => m.Party).NotEmpty();

            RuleFor(m => m.Party.Name).NotEmpty();
            RuleFor(m => m.Party.Address).NotEmpty();
            RuleFor(m => m.Party.Email).NotEmpty();
            RuleFor(m => m.Party.Website).NotEmpty();
            RuleFor(m => m.Party.Phone).NotEmpty();
            RuleFor(m => m.Party.Fax).NotEmpty();

            RuleFor(m => m.Party.Contacts).NotEmpty();

            RuleFor(m => m.Party.Contacts).Must(HavePrimary).WithMessage("One (and only one) contact must be primary");

            RuleFor(m => m.Party.Contacts).NotEmpty();

            RuleForEach(m => m.Party.Contacts).SetValidator(new ContactModelValidator());
        }

        private bool HavePrimary(IEnumerable<ContactModel> contacts)
        {
            if (contacts == null)
                return false;
            int primary = contacts.Count(p => p.IsPrimary);
            return (primary == 1);
        }
    }
}
