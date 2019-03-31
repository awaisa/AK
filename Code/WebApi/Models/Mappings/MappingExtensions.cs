using BusinessCore.Domain;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using WebApiCore.Models.Common;
using WebApiCore.Models.Customer;
using WebApiCore.Models.Financial;
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

        public static Inventory.ItemModel ToModel(this Item entity)
        {
            return entity.MapTo<Item, Inventory.ItemModel>();
        }

        public static Item ToEntity(this Inventory.ItemModel model)
        {
            return model.MapTo<Inventory.ItemModel, Item>();
        }

        public static Item ToEntity(this Inventory.ItemModel model, Item destination)
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

        public static Vendor.VendorModel ToModel(this BusinessCore.Domain.Purchases.Vendor entity)
        {
            return entity.MapTo<BusinessCore.Domain.Purchases.Vendor, Vendor.VendorModel>();
        }

        public static BusinessCore.Domain.Purchases.Vendor ToEntity(this Vendor.VendorModel model)
        {
            return model.MapTo<Vendor.VendorModel, BusinessCore.Domain.Purchases.Vendor>();
        }

        public static BusinessCore.Domain.Purchases.Vendor ToEntity(this Vendor.VendorModel model, BusinessCore.Domain.Purchases.Vendor destination)
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

        #region Reference Controller
        public static Inventory.ItemBrandModel ToModel(this BusinessCore.Domain.Items.ItemBrand entity)
        {
            return entity.MapTo<BusinessCore.Domain.Items.ItemBrand, Inventory.ItemBrandModel>();
        }
        public static Inventory.ItemModelModel ToModel(this BusinessCore.Domain.Items.ItemModel entity)
        {
            return entity.MapTo<BusinessCore.Domain.Items.ItemModel, Inventory.ItemModelModel>();
        }
        public static Inventory.MeasurementModel ToModel(this BusinessCore.Domain.Items.Measurement entity)
        {
            return entity.MapTo<BusinessCore.Domain.Items.Measurement, Inventory.MeasurementModel>();
        }
        public static Common.AccountModel ToModel(this BusinessCore.Domain.Financials.Account entity)
        {
            return entity.MapTo<BusinessCore.Domain.Financials.Account, Common.AccountModel>();
        }
        public static Inventory.ItemTaxGroupModel ToModel(this BusinessCore.Domain.TaxSystem.ItemTaxGroup entity)
        {
            return entity.MapTo<BusinessCore.Domain.TaxSystem.ItemTaxGroup, Inventory.ItemTaxGroupModel>();
        }
        #endregion

        #region Financial Accounts

        public static Financial.SearchRowModel ToRowModel(this BusinessCore.Domain.Financials.Account entity)
        {
            return entity.MapTo<BusinessCore.Domain.Financials.Account, Financial.SearchRowModel>();
        }

        public static FinancialAccountModel ToModel2(this BusinessCore.Domain.Financials.Account entity)
        {
            return entity.MapTo<BusinessCore.Domain.Financials.Account, FinancialAccountModel>();
        }

        //public static BusinessCore.Domain.Financials.Account ToEntity(this VendorModel model)
        //{
        //    return model.MapTo<VendorModel, BusinessCore.Domain.Financials.Account>();
        //}

        //public static BusinessCore.Domain.Financials.Account ToEntity(this VendorModel model, BusinessCore.Domain.Financials.Account destination)
        //{
        //    return model.MapTo(destination);
        //}

        #endregion

        #region Financial Journal
        public static FinancialJournal.SearchRowModel ToRowModel(this JournalEntryHeader entity)
        {
            return entity.MapTo<JournalEntryHeader, FinancialJournal.SearchRowModel>();
        }
        public static FinancialJournal.JournalModel ToModel(this JournalEntryHeader entity)
        {
            return entity.MapTo<JournalEntryHeader, FinancialJournal.JournalModel>();
        }
        public static JournalEntryHeader ToEntity(this FinancialJournal.JournalModel model)
        {
            return model.MapTo<FinancialJournal.JournalModel, JournalEntryHeader>();
        }
        #endregion

        #region References
        public static PaymentTermModel ToModel(this PaymentTerm entity)
        {
            return entity.MapTo<PaymentTerm, PaymentTermModel>();
        }
        #endregion
    }
}