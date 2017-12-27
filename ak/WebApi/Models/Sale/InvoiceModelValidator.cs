using BusinessCore.Services.Inventory;
using BusinessCore.Services.Sales;
using FluentValidation;
using System.Linq;

namespace WebApiCore.Models.Sale
{
    public class InvoiceModelValidator : AbstractValidator<InvoiceModel>
    {
        private IInventoryService _inventoryService;
        private ISalesService _salesService;
        public InvoiceModelValidator(IInventoryService inventoryService, ISalesService salesService)
        {
            _inventoryService = inventoryService;
            _salesService = salesService;

            RuleFor(m => m.No).NotEmpty();
            //RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.VendorId).Must(i => ValidateCustomerId(i.Value)).WithMessage("VendorId is not valid");

            RuleFor(m => m.InvoiceItems)
                .Must(i => i.Any())
                .WithMessage("At least 1 item should exists in invoice");

            RuleForEach(m => m.InvoiceItems).Must(i => ValidateItemId(i.ItemId)).WithMessage("ItemId is not valid");

            RuleFor(m => m.Total).NotEmpty();
        }

        private bool ValidateItemId(int? itemId)
        {
            var item = _inventoryService.GetItemById(itemId ?? 0);
            return item != null;
        }
        private bool ValidateCustomerId(int? customerId)
        {
            var customer = _salesService.GetCustomerById(customerId ?? 0);
            return customer != null;
        }

    }
}
