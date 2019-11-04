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
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
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

            CreateMap<PaymentTermModel, PaymentTerm>().ReverseMap();
        }
    }

}
