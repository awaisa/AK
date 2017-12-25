//-----------------------------------------------------------------------
// <copyright file="ISalesService.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using BusinessCore.Domain;
using BusinessCore.Domain.Sales;
using System.Collections.Generic;
using System.Linq;

namespace BusinessCore.Services.Sales
{
    public partial interface ISalesService
    {
        void AddSalesOrder(SalesOrderHeader salesOrder, bool toSave);
        void UpdateSalesOrder(SalesOrderHeader salesOrder);
        void AddSalesInvoice(SalesInvoiceHeader salesInvoice, int? salesOrderId);
        void AddSalesReceipt(SalesReceiptHeader salesReceipt);
        void AddSalesReceiptNoInvoice(SalesReceiptHeader salesReceipt);
        void AddSalesDelivery(SalesDeliveryHeader salesDelivery, bool toSave);
        IQueryable<SalesInvoiceHeader> GetSalesInvoices();
        SalesInvoiceHeader GetSalesInvoiceById(int id);
        SalesInvoiceHeader GetSalesInvoiceByNo(string no);
        void UpdateSalesInvoice(SalesInvoiceHeader salesInvoice);
        IQueryable<SalesReceiptHeader> GetSalesReceipts();
        SalesReceiptHeader GetSalesReceiptById(int id);
        void UpdateSalesReceipt(SalesReceiptHeader salesReceipt);
        IQueryable<Customer> GetCustomers();
        Customer GetCustomerById(int id);
        //void UpdateCustomer(Customer customer);
        //void AddCustomer(Customer customer);
        Customer SaveCustomer(Customer customer);
        ICollection<SalesReceiptHeader> GetCustomerReceiptsForAllocation(int customerId);
        void SaveCustomerAllocation(CustomerAllocation allocation);
        IQueryable<SalesDeliveryHeader> GetSalesDeliveries();
        IQueryable<SalesOrderHeader> GetSalesOrders();
        SalesOrderHeader GetSalesOrderById(int id);
        SalesDeliveryHeader GetSalesDeliveryById(int id);
        IQueryable<Contact> GetContacts();
        int SaveContact(Contact contact);
        ICollection<SalesInvoiceHeader> GetSalesInvoicesByCustomerId(int customerId, SalesInvoiceStatus status);
        ICollection<CustomerAllocation> GetCustomerAllocations(int customerId);
    }
}
