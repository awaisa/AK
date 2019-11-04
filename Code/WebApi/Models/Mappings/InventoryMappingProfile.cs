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
    public class InventoryMappingProfile : Profile
    {
        public InventoryMappingProfile()
        {
            CreateMap<Item, Inventory.ItemModel>()
                .ForMember(x=>x.TaxGroupId,opt=>opt.MapFrom(mf=>mf.ItemTaxGroupId));
            CreateMap<Inventory.ItemModel, Item>()
                .ForMember(x=>x.ItemTaxGroupId,opt=>opt.MapFrom(mf=>mf.TaxGroupId));
            CreateMap<Item, Inventory.SearchRowModel>();
            

            CreateMap<ItemCategory, ItemCategoryModel>();
            CreateMap<ItemCategoryModel, ItemCategory>();

            CreateMap<ItemBrand, ItemBrandModel>();
            CreateMap<ItemBrandModel, ItemBrand>();

            CreateMap<BusinessCore.Domain.Items.ItemModel, ItemModelModel>();
            CreateMap<ItemModelModel, BusinessCore.Domain.Items.ItemModel>();


            CreateMap<Tax, TaxModel>();
            CreateMap<TaxModel, Tax>();

            CreateMap<TaxModel, SalesInvoiceLineTax>();
                


            CreateMap<TaxGroup, TaxGroupModel>();
            CreateMap<TaxGroupModel, TaxGroup>();
            
            CreateMap<ItemTaxGroup, ItemTaxGroupModel>();

            CreateMap<Measurement, MeasurementModel>();
            CreateMap<MeasurementModel, Measurement>();
        }
    }
}
