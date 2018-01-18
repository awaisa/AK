using System.Linq;
using WebApiCore.Models.Mappings;
using BusinessCore.Services.Inventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiCore.Models.Inventory;
using BusinessCore.Services.Financial;
using WebApiCore.Models.Common;
using BusinessCore.Services.Purchasing;
using WebApiCore.Models.Vendor;

namespace WebApiCore.Controllers
{

    public class ReferenceController : BaseController
    {
        private ILogger<ReferenceController> _log;
        private IInventoryService _service;
        private IFinancialService _financialService;
        private IPurchasingService _purchasingService;

        public ReferenceController(
            IInventoryService service, IFinancialService financialService,IPurchasingService purchasingService,
            ILogger<ReferenceController> log)
        {
            _service = service;
            _financialService = financialService;
            _purchasingService = purchasingService;
            _log = log;
        }

        [HttpGet("GetCatagory")]
        [Produces(typeof(ItemCategoryModel))]
        public IActionResult GetCatagory()
        {
            var catagories = _service.GetItemCategories().Select(c => c.ToModel());

            return Ok(catagories);
        }

        [HttpGet("GetBrand")]
        [Produces(typeof(ItemBrandModel))]

        public IActionResult GetBrand()
        {
            var brands = _service.GetItemBrands().Select(c => c.ToModel());
            return Ok(brands);
        }

        [HttpGet("GetModel")]
        [Produces(typeof(ItemModelModel))]
        public IActionResult GetModel()
        {
            var models = _service.GetItemModels().Select(c => c.ToModel());
            return Ok(models);
        }

        [HttpGet("GetTaxGroup")] 
        [Produces(typeof(ItemTaxGroupModel))]
        public IActionResult GetTaxGroup()
        {
            var taxGroups = _service.GetItemTaxGroups().Select(x=>x.ToModel());
            return Ok(taxGroups);
        }

        [HttpGet("GetMeasuremets")]
        [Produces(typeof(MeasurementModel))]
        public IActionResult GetMeasuremets()
        {
            var measurements = _service.GetMeasurements().Select(c => c.ToModel());
            return Ok(measurements);
        }

        [HttpGet("GetAccounts")]
        [Produces(typeof(AccountModel))]
        public IActionResult GetAccounts()
        {
            var accounts = _financialService.GetAccounts().Select(c => c.ToModel());
            return Ok(accounts);
        }

        [HttpGet("GetVendors")]
        [Produces(typeof(VendorModel))]
        public IActionResult GetVendors()
        {
            var vendors = _purchasingService.GetVendors().Select(c => c.ToModel());
            return Ok(vendors);
        }
    }
}