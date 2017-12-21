using BusinessCore.Services.Inventory;
using FluentValidation;
using System.Linq;

namespace WebApiCore.Models.Purchase
{
    public class InvoiceModelValidator : AbstractValidator<InvoiceModel>
    {
        private IInventoryService _service;
        public InvoiceModelValidator(IInventoryService service)
        {
            _service = service;

            RuleFor(m => m.No).NotEmpty();
            //RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.InvoiceItems)
                .Must(i => i.Any())
                .WithMessage("At least 1 item should exists in invoice");

            RuleForEach(m => m.InvoiceItems).Must(i => ValidateItemId(i.ItemId)).WithMessage("ItemId is not valid");

            RuleFor(m => m.Total).NotEmpty();
        }

        private bool ValidateItemId(int? itemId)
        {
            var item = _service.GetItemById(itemId ?? 0);
            return item != null;
        }

    }
}
