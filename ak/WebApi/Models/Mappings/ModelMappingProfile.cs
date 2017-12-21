using AutoMapper;
using BusinessCore.Domain;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.TaxSystem;
using System.Linq;
using WebApiCore.Models.Common;
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
            CreateMap<ItemModel, Item>();

            CreateMap<ItemCategory, ItemCategoryModel>();
            CreateMap<ItemCategoryModel, ItemCategory>();

            CreateMap<Brand, ItemBrandModel>();
            CreateMap<ItemBrandModel, Brand>();

            CreateMap<Model, ItemModelModel>();
            CreateMap<ItemModelModel, Model>();

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

            CreateMap<BusinessCore.Domain.Purchases.Vendor, VendorModel>()
                .ForMember(x => x.PartyId, opt => opt.MapFrom(mf => mf.Party.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(mf => mf.Party.Name))
                .ForMember(x => x.Email, opt => opt.MapFrom(mf => mf.Party.Email))
                .ForMember(x => x.Website, opt => opt.MapFrom(mf => mf.Party.Website))
                .ForMember(x => x.Phone, opt => opt.MapFrom(mf => mf.Party.Phone))
                .ForMember(x => x.Fax, opt => opt.MapFrom(mf => mf.Party.Fax))
                .ForMember(x => x.Address, opt => opt.MapFrom(mf => mf.Party.Address))
                //.ForMember(x=> x.PrimaryContact, opt => opt.MapFrom(mf=> mf.PrimaryContact))
                .ForMember(x=> x.Contacts, opt => opt.MapFrom(mf=> mf.Party.Contacts));

            CreateMap<Contact, ContactModel>();
            CreateMap<ContactModel, Contact>();

            CreateMap<VendorModel, BusinessCore.Domain.Purchases.Vendor>()
                .ForPath(x => x.Party.Id, opt => opt.MapFrom(mf => mf.PartyId))
                .ForPath(x => x.Party.Name, opt => opt.MapFrom(mf => mf.Name))
                .ForPath(x => x.Party.Email, opt => opt.MapFrom(mf => mf.Email))
                .ForPath(x => x.Party.Website, opt => opt.MapFrom(mf => mf.Website))
                .ForPath(x => x.Party.Phone, opt => opt.MapFrom(mf => mf.Phone))
                .ForPath(x => x.Party.Fax, opt => opt.MapFrom(mf => mf.Fax))
                .ForPath(x => x.Party.Address, opt => opt.MapFrom(mf => mf.Address))
                .ForPath(x => x.Party.Contacts, opt => opt.MapFrom(mf => mf.Contacts));
                //.ForPath(x => x.PrimaryContact, opt => opt.MapFrom(mf => mf.PrimaryContact));

        }
    }
}
