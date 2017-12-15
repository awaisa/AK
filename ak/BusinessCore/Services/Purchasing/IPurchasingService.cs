using BusinessCore.Domain.Purchases;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessCore.Services.Purchasing
{
    public partial interface IPurchasingService
    {
        void AddPurchaseInvoice(PurchaseInvoiceHeader purchaseIvoice, int? purchaseOrderId);
        void AddPurchaseOrder(PurchaseOrderHeader purchaseOrder, bool toSave);
        void AddPurchaseOrderReceipt(PurchaseReceiptHeader purchaseOrderReceipt);
        IQueryable<Vendor> GetVendors();
        Vendor GetVendorById(int id);
        IQueryable<PurchaseOrderHeader> GetPurchaseOrders();
        PurchaseOrderHeader GetPurchaseOrderById(int id);
        PurchaseReceiptHeader GetPurchaseReceiptById(int id);
        void AddVendor(Vendor vendor);
        void UpdateVendor(Vendor vendor);
        IQueryable<PurchaseInvoiceHeader> GetPurchaseInvoices();
        PurchaseInvoiceHeader GetPurchaseInvoiceById(int id);
        void SavePayment(int invoiceId, int vendorId, int accountId, decimal amount, DateTime date);
    }
}
