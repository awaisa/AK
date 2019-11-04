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
    public class SaleMappingProfile : Profile
    {
        public SaleMappingProfile()
        {
            CreateMap<SalesInvoiceHeader, Sale.InvoiceModel>()
                .ForMember(x => x.InvoiceItems, opt => opt.MapFrom(mf => mf.SalesInvoiceLines))
                .ForMember(x => x.Total, opt => opt.MapFrom(mf => mf.ComputeTotalAmount()));
            CreateMap<SalesInvoiceHeader, Sale.SearchRowModel>();

            CreateMap<Sale.InvoiceModel, SalesInvoiceHeader>()
                .ForPath(x => x.SalesInvoiceLines, opt => opt.MapFrom(mf => mf.InvoiceItems));
            //.ForPath(x => x.ComputeTotalAmount(), opt => opt.MapFrom(mf => mf.Total));

            CreateMap<Sale.InvoiceItemModel, SalesInvoiceLine>()
                .ForPath(x => x.DiscountAmount, opt => opt.MapFrom(mf => mf.Discount))
                .ForPath(x => x.Taxes, opt => opt.MapFrom(mf => mf.Taxes))
                ;

            CreateMap<TaxModel, SalesInvoiceLineTax>()
                .ForPath(x => x.TaxId, opt => opt.MapFrom(mf => mf.Id))
                .ForPath(x => x.Id, opt => opt.Ignore());
            CreateMap<SalesInvoiceLineTax, TaxModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(mf => mf.TaxId));
        }
    }
}
