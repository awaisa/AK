using BusinessCore.Domain.Items;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore.Models.Purchase
{
    public class InvoiceModelValidator : AbstractValidator<InvoiceModel>
    {
        public InvoiceModelValidator()
        {
            //RuleFor(m => m.Code).NotEmpty();
            //RuleFor(m => m.Description).NotEmpty();
            //RuleFor(m => m.SellDescription).NotEmpty();
            //RuleFor(m => m.PurchaseDescription).NotEmpty();
            //RuleFor(m => m.Price).NotEmpty();
            //RuleFor(m => m.Cost).NotEmpty();
        }
    }
}
