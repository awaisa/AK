//-----------------------------------------------------------------------
// <copyright file="IPurchasingService.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using BusinessCore.Domain.Purchases;
using System;
using System.Collections.Generic;

namespace BusinessCore.Services.Purchasing
{
    public partial interface IPurchasingService
    {
        void AddPurchaseInvoice(PurchaseInvoiceHeader purchaseIvoice, int? purchaseOrderId);
        void AddPurchaseOrder(PurchaseOrderHeader purchaseOrder, bool toSave);
        void AddPurchaseOrderReceipt(PurchaseReceiptHeader purchaseOrderReceipt);
        IEnumerable<Vendor> GetVendors();
        Vendor GetVendorById(int id);
        IEnumerable<PurchaseOrderHeader> GetPurchaseOrders();
        PurchaseOrderHeader GetPurchaseOrderById(int id);
        PurchaseReceiptHeader GetPurchaseReceiptById(int id);
        void AddVendor(Vendor vendor);
        void UpdateVendor(Vendor vendor);
        IEnumerable<PurchaseInvoiceHeader> GetPurchaseInvoices();
        PurchaseInvoiceHeader GetPurchaseInvoiceById(int id);
        void SavePayment(int invoiceId, int vendorId, int accountId, decimal amount, DateTime date);
    }
}
