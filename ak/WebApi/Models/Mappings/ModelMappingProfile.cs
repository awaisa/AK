using AutoMapper;
using BusinessCore.Domain;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
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

            CreateMap<PurchaseInvoiceHeader, InvoiceModel>();
            CreateMap<InvoiceModel, PurchaseInvoiceHeader>();

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
                .ForMember(x=> x.PrimaryContact, opt => opt.MapFrom(mf=> mf.PrimaryContact))
                .ForMember(x=> x.Contacts, opt => opt.MapFrom(mf=> mf.Party.Contacts));

            CreateMap<VendorModel, BusinessCore.Domain.Purchases.Vendor>()
                .ForPath(x => x.Party.Id, opt => opt.MapFrom(mf => mf.PartyId))
                .ForPath(x => x.Party.Name, opt => opt.MapFrom(mf => mf.Name))
                .ForPath(x => x.Party.Email, opt => opt.MapFrom(mf => mf.Email))
                .ForPath(x => x.Party.Website, opt => opt.MapFrom(mf => mf.Website))
                .ForPath(x => x.Party.Phone, opt => opt.MapFrom(mf => mf.Phone))
                .ForPath(x => x.Party.Fax, opt => opt.MapFrom(mf => mf.Fax))
                .ForPath(x => x.Party.Address, opt => opt.MapFrom(mf => mf.Address))
                .ForPath(x => x.Party.Contacts, opt => opt.MapFrom(mf => mf.Contacts))
                .ForMember(x => x.PrimaryContact, opt => opt.MapFrom(mf => mf.PrimaryContact));
        }
    }
}
