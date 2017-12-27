using BusinessCore.Services.Inventory;
using BusinessCore.Services.Purchasing;
using FluentValidation;
using System.Linq;

namespace WebApiCore.Models.Purchase
{
    public class InvoiceModelValidator : AbstractValidator<InvoiceModel>
    {
        private IInventoryService _inventoryService;
        private IPurchasingService _purchasingService;
        public InvoiceModelValidator(IInventoryService inventoryService,IPurchasingService purchasingService)
        {
            _inventoryService = inventoryService;
            _purchasingService = purchasingService;

            RuleFor(m => m.No).NotEmpty();
            //RuleFor(m => m.Description).NotEmpty();
            RuleFor(m => m.VendorId).Must(i => ValidateVendorId(i.Value)).WithMessage("VendorId is not valid");

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
        private bool ValidateVendorId(int? vendorId)
        {
            var vendor = _purchasingService.GetVendorById(vendorId ?? 0);
            return vendor != null;
        }

    }
}
