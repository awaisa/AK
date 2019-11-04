using AutoMapper;
using BusinessCore.Domain;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.Sales;
using BusinessCore.Domain.TaxSystem;
using System.Linq;
using WebApiCore.Models.Common;
using WebApiCore.Models.Customer;
using WebApiCore.Models.Financial;
using WebApiCore.Models.Inventory;
using WebApiCore.Models.Purchase;
using WebApiCore.Models.Vendor;

namespace WebApiCore.Models.Mappings
{
    public class PurchaseMappingProfile : Profile
    {
        public PurchaseMappingProfile()
        {
            CreateMap<PurchaseInvoiceHeader, InvoiceModel>()
                 .ForMember(x => x.InvoiceItems, opt => opt.MapFrom(mf => mf.PurchaseInvoiceLines));
            CreateMap<InvoiceModel, PurchaseInvoiceHeader>()
                .ForPath(x => x.PurchaseInvoiceLines, opt => opt.MapFrom(mf => mf.InvoiceItems));

            CreateMap<PurchaseInvoiceLine, InvoiceItemModel>();
            CreateMap<InvoiceItemModel, PurchaseInvoiceLine>();

        }
    }
}
