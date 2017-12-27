using BusinessCore.Domain;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using WebApiCore.Models.Common;
using WebApiCore.Models.Customer;
using WebApiCore.Models.Inventory;
using WebApiCore.Models.Purchase;
using WebApiCore.Models.Vendor;

namespace WebApiCore.Models.Mappings
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region Item
        public static Inventory.SearchRowModel ToRowModel(this Item entity)
        {
            return entity.MapTo<Item, Inventory.SearchRowModel>();
        }

        public static ItemModel ToModel(this Item entity)
        {
            return entity.MapTo<Item, ItemModel>();
        }

        public static Item ToEntity(this ItemModel model)
        {
            return model.MapTo<ItemModel, Item>();
        }

        public static Item ToEntity(this ItemModel model, Item destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Item Category

        public static ItemCategoryModel ToModel(this ItemCategory entity)
        {
            return entity.MapTo<ItemCategory, ItemCategoryModel>();
        }

        public static ItemCategory ToEntity(this ItemCategoryModel model)
        {
            return model.MapTo<ItemCategoryModel, ItemCategory>();
        }

        public static ItemCategory ToEntity(this ItemCategoryModel model, ItemCategory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Purchase

        public static InvoiceModel ToModel(this PurchaseInvoiceHeader entity)
        {
            return entity.MapTo<PurchaseInvoiceHeader, InvoiceModel>();
        }

        public static PurchaseInvoiceHeader ToEntity(this InvoiceModel model)
        {
            return model.MapTo<InvoiceModel, PurchaseInvoiceHeader>();
        }

        public static PurchaseInvoiceHeader ToEntity(this InvoiceModel model, PurchaseInvoiceHeader destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Purchase Item

        public static InvoiceItemModel ToModel(this PurchaseInvoiceLine entity)
        {
            return entity.MapTo<PurchaseInvoiceLine, InvoiceItemModel>();
        }

        public static PurchaseInvoiceLine ToEntity(this InvoiceItemModel model)
        {
            return model.MapTo<InvoiceItemModel, PurchaseInvoiceLine>();
        }

        public static PurchaseInvoiceLine ToEntity(this InvoiceItemModel model, PurchaseInvoiceLine destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Vendor

        public static Vendor.SearchRowModel ToRowModel(this BusinessCore.Domain.Purchases.Vendor entity)
        {
            return entity.MapTo<BusinessCore.Domain.Purchases.Vendor, Vendor.SearchRowModel>();
        }

        public static VendorModel ToModel(this BusinessCore.Domain.Purchases.Vendor entity)
        {
            return entity.MapTo<BusinessCore.Domain.Purchases.Vendor, VendorModel>();
        }

        public static BusinessCore.Domain.Purchases.Vendor ToEntity(this VendorModel model)
        {
            return model.MapTo<VendorModel, BusinessCore.Domain.Purchases.Vendor>();
        }

        public static BusinessCore.Domain.Purchases.Vendor ToEntity(this VendorModel model, BusinessCore.Domain.Purchases.Vendor destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Customer
        public static Customer.SearchRowModel ToRowModel(this BusinessCore.Domain.Sales.Customer entity)
        {
            return entity.MapTo<BusinessCore.Domain.Sales.Customer, Customer.SearchRowModel>();
        }
        public static CustomerModel ToModel(this BusinessCore.Domain.Sales.Customer entity)
        {
            return entity.MapTo<BusinessCore.Domain.Sales.Customer, CustomerModel>();
        }

        public static BusinessCore.Domain.Sales.Customer ToEntity(this CustomerModel model)
        {
            return model.MapTo<CustomerModel, BusinessCore.Domain.Sales.Customer>();
        }

        public static BusinessCore.Domain.Sales.Customer ToEntity(this CustomerModel model, BusinessCore.Domain.Sales.Customer destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Sale Invoice
        public static Sale.SearchRowModel ToRowModel(this BusinessCore.Domain.Sales.SalesInvoiceHeader entity)
        {
            return entity.MapTo<BusinessCore.Domain.Sales.SalesInvoiceHeader, Sale.SearchRowModel>();
        }
        public static Sale.InvoiceModel ToModel(this BusinessCore.Domain.Sales.SalesInvoiceHeader entity)
        {
            return entity.MapTo<BusinessCore.Domain.Sales.SalesInvoiceHeader, Sale.InvoiceModel>();
        }

        public static BusinessCore.Domain.Sales.SalesInvoiceHeader ToEntity(this Sale.InvoiceModel model)
        {
            return model.MapTo<Sale.InvoiceModel, BusinessCore.Domain.Sales.SalesInvoiceHeader>();
        }

        public static BusinessCore.Domain.Sales.SalesInvoiceHeader ToEntity(this Sale.InvoiceModel model, BusinessCore.Domain.Sales.SalesInvoiceHeader destination)
        {
            return model.MapTo(destination);
        }
        #endregion
    }
}