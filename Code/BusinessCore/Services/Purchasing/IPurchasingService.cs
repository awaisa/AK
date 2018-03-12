using BusinessCore.Domain;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.TaxSystem;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessCore.Services.Purchasing
{
    public partial interface IPurchasingService
    {
        //void AddPurchaseInvoice(PurchaseInvoiceHeader purchaseIvoice, int? purchaseOrderId);
        //void UpdatePurchaseInvoice(PurchaseInvoiceHeader purchaseIvoice, int? purchaseOrderId);
        void SavePurchaseInvoice(PurchaseInvoiceHeader purchaseIvoice);
        void AddPurchaseOrder(PurchaseOrderHeader purchaseOrder, bool toSave);
        void AddPurchaseOrderReceipt(PurchaseReceiptHeader purchaseOrderReceipt);
        IQueryable<Measurement> GetItems();
        IQueryable<Vendor> GetVendors();
        IQueryable<TaxGroupTax> GetTaxGroupTax();
        IQueryable<TaxGroup> GetTaxGroup();
        IQueryable<Tax> GetTaxes();
        IQueryable<PaymentTerm> GetPaymentTerms();
        Vendor GetVendorById(int id);
        void DeleteVendor(int id);
        void UpdateVendor(Vendor vendor);
        IQueryable<PurchaseOrderHeader> GetPurchaseOrders();
        PurchaseOrderHeader GetPurchaseOrderById(int id);
        PurchaseReceiptHeader GetPurchaseReceiptById(int id);
        Vendor SaveVendor(Vendor vendor);
        IQueryable<PurchaseInvoiceHeader> GetPurchaseInvoices();
        PurchaseInvoiceHeader GetPurchaseInvoiceById(int id);
        void SavePayment(int invoiceId, int vendorId, int accountId, decimal amount, DateTime date);
    }
}
