using BusinessCore.Domain.Items;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore.Models.Inventory
{
    public class ItemModelValidator : AbstractValidator<ItemModel>
    {
        public ItemModelValidator()
        {
            RuleFor(m => m.Code).NotEmpty().NotNull();
            RuleFor(m => m.Description).NotEmpty().NotNull();
            RuleFor(m => m.SellDescription).NotEmpty().NotNull();
            RuleFor(m => m.PurchaseDescription).NotEmpty().NotNull();
            RuleFor(m => m.Price).NotEmpty().NotNull();
            RuleFor(m => m.Cost).NotEmpty().NotNull();
        }
    }
}
