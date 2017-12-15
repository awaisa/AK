using BusinessCore.Domain.Items;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore.Models.Common;

namespace WebApiCore.Models.Vendor
{
    public class VendorModelValidator : AbstractValidator<VendorModel>
    {
        public VendorModelValidator()
        {
            RuleFor(m => m.No).NotEmpty();
            RuleFor(m => m.Name).NotEmpty();
            RuleFor(m => m.Address).NotEmpty();
            RuleFor(m => m.Email).NotEmpty();
            RuleFor(m => m.Website).NotEmpty();
            RuleFor(m => m.Phone).NotEmpty();
            RuleFor(m => m.Fax).NotEmpty();
            RuleFor(m => m.Contacts).NotEmpty();
            RuleFor(m => m.Contacts).Must(HavePrimary).WithMessage("One (and only one) contact must be primary");
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
