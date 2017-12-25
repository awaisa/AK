using AutoMapper;
using BusinessCore.Domain;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.TaxSystem;
using System.Linq;
using WebApiCore.Models.Common;
using WebApiCore.Models.Customer;
using WebApiCore.Models.Inventory;
using WebApiCore.Models.Purchase;
using WebApiCore.Models.Vendor;

namespace WebApiCore.Models.Mappings
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<Item, ItemModel>();
            CreateMap<Item, Inventory.SearchRowModel>();
            CreateMap<ItemModel, Item>();

            CreateMap<ItemCategory, ItemCategoryModel>();
            CreateMap<ItemCategoryModel, ItemCategory>();

            CreateMap<Brand, ItemBrandModel>();
            CreateMap<ItemBrandModel, Brand>();

            CreateMap<Model, ItemModelModel>();
            CreateMap<ItemModelModel, Model>();


            CreateMap<Tax, TaxModel>();
            CreateMap<TaxModel, Tax>();

            CreateMap<TaxGroup, TaxGroupModel>();
            CreateMap<TaxGroupModel, TaxGroup>();

            CreateMap<Measurement, MeasurementModel>();
            CreateMap<MeasurementModel, Measurement>();

            CreateMap<Account, AccountModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(mf => mf.AccountName))
                .ForMember(x => x.Code, opt => opt.MapFrom(mf => mf.AccountCode));
            CreateMap<AccountModel, Account>()
                .ForPath(x => x.AccountName, opt => opt.MapFrom(mf => mf.Name))
                .ForPath(x => x.AccountCode, opt => opt.MapFrom(mf => mf.Code));

            CreateMap<PurchaseInvoiceHeader, InvoiceModel>()
                 .ForMember(x => x.InvoiceItems, opt => opt.MapFrom(mf => mf.PurchaseInvoiceLines));
            CreateMap<InvoiceModel, PurchaseInvoiceHeader>()
                .ForPath(x => x.PurchaseInvoiceLines, opt => opt.MapFrom(mf => mf.InvoiceItems));
            
            CreateMap<PurchaseInvoiceLine, InvoiceItemModel>();
            CreateMap<InvoiceItemModel, PurchaseInvoiceLine>();

            CreateMap<Party, PartyModel>();
            CreateMap<PartyModel, Party>();

            CreateMap<BusinessCore.Domain.Purchases.Vendor, VendorModel>();
            CreateMap<BusinessCore.Domain.Purchases.Vendor, Vendor.SearchRowModel>();
            CreateMap<VendorModel, BusinessCore.Domain.Purchases.Vendor>();

            CreateMap<Contact, ContactModel>();
            CreateMap<ContactModel, Contact>();

            CreateMap<BusinessCore.Domain.Sales.Customer, CustomerModel>();
            CreateMap<BusinessCore.Domain.Sales.Customer, Customer.SearchRowModel>();
            CreateMap<CustomerModel, BusinessCore.Domain.Sales.Customer>();
        }
    }
}
