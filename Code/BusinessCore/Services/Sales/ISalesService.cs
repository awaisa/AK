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
        SalesInvoiceHeader SaveSaleInvoice(SalesInvoiceHeader salesInvoice);
        IQueryable<SalesInvoiceHeader> GetSalesInvoices();
        SalesInvoiceHeader GetSalesInvoiceById(int id);
        //SalesInvoiceHeader GetSalesInvoiceByNo(string no);
        void AddSalesReceipt(SalesReceiptHeader salesReceipt);
        void AddSalesReceiptNoInvoice(SalesReceiptHeader salesReceipt);
        void AddSalesDelivery(SalesDeliveryHeader salesDelivery, bool toSave);
        IQueryable<SalesReceiptHeader> GetSalesReceipts();
        SalesReceiptHeader GetSalesReceiptById(int id);
        void UpdateSalesReceipt(SalesReceiptHeader salesReceipt);
        IQueryable<Customer> GetCustomers();
        Customer GetCustomerById(int id);
        //void UpdateCustomer(Customer customer);
        //void AddCustomer(Customer customer);
        Customer SaveCustomer(Customer customer);
        void DeleteCustomer(int cutomerId);
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
