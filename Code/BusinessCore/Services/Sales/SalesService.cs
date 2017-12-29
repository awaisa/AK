//-----------------------------------------------------------------------
// <copyright file="SalesService.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using BusinessCore.Data;
using BusinessCore.Domain;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Sales;
using BusinessCore.Services.Financial;
using BusinessCore.Services.Inventory;
using System.Linq;
using System;
using System.Collections.Generic;
using BusinessCore.Domain.TaxSystem;
using BusinessCore.Services.Security;

namespace BusinessCore.Services.Sales
{
    public partial class SalesService : BaseService, ISalesService
    {
        private readonly IFinancialService _financialService;
        private readonly IInventoryService _inventoryService;

        private readonly IRepository<SalesOrderHeader> _salesOrderRepo;
        private readonly IRepository<SalesInvoiceHeader> _salesInvoiceRepo;
        private readonly IRepository<SalesReceiptHeader> _salesReceiptRepo;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<Measurement> _measurementRepo;
        private readonly IRepository<SequenceNumber> _sequenceNumberRepo;
        private readonly IRepository<PaymentTerm> _paymentTermRepo;
        private readonly IRepository<SalesDeliveryHeader> _salesDeliveryRepo;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IRepository<GeneralLedgerSetting> _genetalLedgerSetting;
        private readonly IRepository<Contact> _contactRepo;
        private readonly IRepository<TaxGroup> _taxGroupRepo;

        public SalesService(IFinancialService financialService,
            IInventoryService inventoryService,
            IRepository<SalesOrderHeader> salesOrderRepo,
            IRepository<SalesInvoiceHeader> salesInvoiceRepo,
            IRepository<SalesReceiptHeader> salesReceiptRepo,
            IRepository<Customer> customerRepo,
            IRepository<Account> accountRepo,
            IRepository<Item> itemRepo,
            IRepository<Measurement> measurementRepo,
            IRepository<SequenceNumber> sequenceNumberRepo,
            IRepository<PaymentTerm> paymentTermRepo,
            IRepository<SalesDeliveryHeader> salesDeliveryRepo,
            IRepository<Bank> bankRepo,
            IRepository<GeneralLedgerSetting> generalLedgerSetting,
            IRepository<Contact> contactRepo,
            IRepository<TaxGroup> taxGroupRepo)
            : base(sequenceNumberRepo, generalLedgerSetting, paymentTermRepo, bankRepo)
        {
            _financialService = financialService;
            _inventoryService = inventoryService;

            _salesOrderRepo = salesOrderRepo;
            _salesInvoiceRepo = salesInvoiceRepo;
            _salesReceiptRepo = salesReceiptRepo;
            _customerRepo = customerRepo;
            _accountRepo = accountRepo;
            _itemRepo = itemRepo;
            _measurementRepo = measurementRepo;
            _sequenceNumberRepo = sequenceNumberRepo;
            _paymentTermRepo = paymentTermRepo;
            _salesDeliveryRepo = salesDeliveryRepo;
            _bankRepo = bankRepo;
            _genetalLedgerSetting = generalLedgerSetting;
            _contactRepo = contactRepo;
            _taxGroupRepo = taxGroupRepo;
        }

        #region CUSTOMER
        public IQueryable<Customer> GetCustomers()
        {
            System.Linq.Expressions.Expression<Func<Customer, object>>[] includeProperties =
            {
                p => p.Party,
                c => c.AccountsReceivableAccount,
                p => p.PaymentTerm,
                //p => p.PrimaryContact,
                p => p.TaxGroup
            };

            var customers = _customerRepo.GetAllIncluding(includeProperties);

            return customers;
        }
        public Customer GetCustomerById(int id)
        {
            System.Linq.Expressions.Expression<Func<Customer, object>>[] includeProperties =
            {
                p => p.Party,
                p => p.Party.Contacts,
                c => c.AccountsReceivableAccount,
                p => p.PaymentTerm,
                p => p.TaxGroup
            };

            var customer = _customerRepo.GetAllIncluding(includeProperties)
                .Where(c => c.Id == id).FirstOrDefault();

            return customer;
        }
        public Customer SaveCustomer(Customer customer)
        {
            var dbObject = GetCustomerById(customer.Id);
            #region UPDATE
            if (dbObject != null)
            {
                dbObject.No = customer.No;
                dbObject.TaxGroupId = customer.TaxGroupId;
                dbObject.PaymentTermId = customer.PaymentTermId;
                dbObject.TaxGroupId = customer.TaxGroupId;

                dbObject.AccountsReceivableAccountId = customer.AccountsReceivableAccountId;
                dbObject.SalesAccountId = customer.SalesAccountId;
                dbObject.SalesDiscountAccountId = customer.SalesDiscountAccountId;
                dbObject.PromptPaymentDiscountAccountId = customer.PromptPaymentDiscountAccountId;
                dbObject.CustomerAdvancesAccountId = customer.CustomerAdvancesAccountId;

                dbObject.IsActive = customer.IsActive;

                dbObject.Party.Address = customer.Party.Address;
                dbObject.Party.Email = customer.Party.Email;
                dbObject.Party.Name = customer.Party.Name;
                dbObject.Party.Phone = customer.Party.Phone;
                dbObject.Party.Fax = customer.Party.Fax;
                dbObject.Party.IsActive = customer.Party.IsActive;

                var contactsToUpdateIds = dbObject.Party.Contacts.Select(c => c.Id).ToList();
                foreach (var contact in customer.Party.Contacts)
                {
                    var existingContact = dbObject.Party.Contacts.FirstOrDefault(c => c.Id == contact.Id);
                    if (existingContact != null)
                    {
                        existingContact.FirstName = contact.FirstName;
                        existingContact.LastName = contact.LastName;
                        existingContact.MiddleName = contact.MiddleName;
                        existingContact.IsPrimary = contact.IsPrimary;
                        existingContact.IsActive = contact.IsActive;

                        contactsToUpdateIds.Remove(existingContact.Id);
                    }
                    else
                    {
                        dbObject.Party.Contacts.Add(new Contact()
                        {
                            ContactType = ContactTypes.Customer,
                            Party = dbObject.Party,
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            MiddleName = contact.MiddleName,
                            IsPrimary = contact.IsPrimary
                        });
                    }
                }
                foreach (var contact in dbObject.Party.Contacts.Where(c => contactsToUpdateIds.Contains(c.Id)))
                {
                    contact.Deleted = true;
                }

                _customerRepo.Update(dbObject);
                customer = dbObject;
            }
            #endregion
            #region INSERT
            else
            {
                customer.No = GetNextNumber(SequenceNumberTypes.Customer).ToString();

                var accountAR = _accountRepo.Table.Where(e => e.AccountCode == AccountCodes.AccountsReceivable_10120).FirstOrDefault();
                var accountSales = _accountRepo.Table.Where(e => e.AccountCode == AccountCodes.Sales_40100).FirstOrDefault();
                var accountAdvances = _accountRepo.Table.Where(e => e.AccountCode == AccountCodes.CutomerAdvances_20120).FirstOrDefault();
                var accountSalesDiscount = _accountRepo.Table.Where(e => e.AccountCode == AccountCodes.SalesDiscount_40400).FirstOrDefault();

                customer.AccountsReceivableAccountId = accountAR != null ? (int?)accountAR.Id : null;
                customer.SalesAccountId = accountSales != null ? (int?)accountSales.Id : null;
                customer.CustomerAdvancesAccountId = accountAdvances != null ? (int?)accountAdvances.Id : null;
                customer.SalesDiscountAccountId = accountSalesDiscount != null ? (int?)accountSalesDiscount.Id : null;
                customer.TaxGroupId = _taxGroupRepo.Table.Where(tg => tg.Description == "VAT").FirstOrDefault().Id;

                customer.Party.PartyType = PartyTypes.Customer;
                foreach (var contact in customer.Party.Contacts)
                {
                    contact.Party = customer.Party;
                    contact.ContactType = ContactTypes.Customer;
                }

                _customerRepo.Insert(customer);
            }
            #endregion
            return customer;
        }
        public void DeleteCustomer(int cutomerId)
        {
            var dbObject = GetCustomerById(cutomerId);
            if (dbObject != null)
            {
                dbObject.Deleted = true;
                _customerRepo.Update(dbObject);
            }
        }
        #endregion

        #region SALE INVOICE
        public IQueryable<SalesInvoiceHeader> GetSalesInvoices()
        {
            System.Linq.Expressions.Expression<Func<SalesInvoiceHeader, object>>[] includeProperties =
            {
                p => p.SalesInvoiceLines,
                p => p.SalesInvoiceLines.Select(p2 => p2.Item)
            };
            var query = _salesInvoiceRepo.GetAllIncluding(includeProperties);
            return query;
        }
        public SalesInvoiceHeader GetSalesInvoiceById(int id)
        {
            System.Linq.Expressions.Expression<Func<SalesInvoiceHeader, object>>[] includeProperties =
            {
                p => p.SalesInvoiceLines,
                p => p.SalesInvoiceLines.Select(p2 => p2.Item)
            };

            var salesInvoice = _salesInvoiceRepo.GetAllIncluding(includeProperties)
                .Where(c => c.Id == id).FirstOrDefault();
            return salesInvoice;
        }
        //public SalesInvoiceHeader GetSalesInvoiceByNo(string no)
        //{
        //    var query = from invoice in _salesInvoiceRepo.Table
        //                where invoice.No == no
        //                select invoice;
        //    return query.FirstOrDefault();
        //}
        public SalesInvoiceHeader SaveSaleInvoice(SalesInvoiceHeader salesInvoice)
        {
            decimal totalAmount = 0, totalDiscount = 0;
            var taxes = new List<KeyValuePair<int, decimal>>();
            var sales = new List<KeyValuePair<int, decimal>>();
            var glHeader = _financialService.CreateGeneralLedgerHeader(DocumentTypes.SalesInvoice, salesInvoice.Date, salesInvoice.Description);
            var customer = _customerRepo.GetById(salesInvoice.CustomerId);

            foreach (var lineItem in salesInvoice.SalesInvoiceLines)
            {
                var item = _itemRepo.GetById(lineItem.ItemId);

                var lineAmount = lineItem.Quantity * lineItem.Amount;

                if (!item.GLAccountsValidated())
                    throw new Exception("Item is not correctly setup for financial transaction. Please check if GL accounts are all set.");

                lineItem.DiscountAmount = (lineItem.Discount / 100) * lineAmount;
                totalDiscount += lineItem.DiscountAmount;

                var totalLineAmount = lineAmount - lineItem.DiscountAmount;

                totalAmount += totalLineAmount;

                var lineTaxes = _financialService.ComputeOutputTax(salesInvoice.CustomerId, item.Id, lineItem.Quantity, lineItem.Amount, lineItem.Discount);

                foreach (var t in lineTaxes)
                    taxes.Add(t);

                lineItem.TaxAmount = lineTaxes != null && lineTaxes.Count > 0 ? lineTaxes.Sum(t => t.Value) : 0;
                totalLineAmount = totalLineAmount - lineItem.TaxAmount;

                sales.Add(new KeyValuePair<int, decimal>(item.SalesAccountId.Value, totalLineAmount));

                if (item.ItemCategory.ItemType == ItemTypes.Purchased)
                {
                    lineItem.InventoryControlJournal = _inventoryService.CreateInventoryControlJournal(lineItem.ItemId,
                        lineItem.MeasurementId,
                        DocumentTypes.SalesInvoice,
                        null,
                        lineItem.Quantity,
                        lineItem.Quantity * item.Cost,
                        lineItem.Quantity * item.Price);
                }
            }

            totalAmount += salesInvoice.ShippingHandlingCharge;
            var debitCustomerAR = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Dr, customer.AccountsReceivableAccount.Id, Math.Round(totalAmount, 2, MidpointRounding.ToEven));
            glHeader.GeneralLedgerLines.Add(debitCustomerAR);

            var groupedSalesAccount = from s in sales
                                      group s by s.Key into grouped
                                      select new
                                      {
                                          Key = grouped.Key,
                                          Value = grouped.Sum(s => s.Value)
                                      };

            foreach (var salesAccount in groupedSalesAccount)
            {
                var salesAmount = salesAccount.Value;
                var creditSalesAccount = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Cr, salesAccount.Key, Math.Round(salesAmount, 2, MidpointRounding.ToEven));
                glHeader.GeneralLedgerLines.Add(creditSalesAccount);
            }

            if (taxes != null && taxes.Count > 0)
            {
                var groupedTaxes = from t in taxes
                                   group t by t.Key into grouped
                                   select new
                                   {
                                       Key = grouped.Key,
                                       Value = grouped.Sum(t => t.Value)
                                   };

                foreach (var tax in groupedTaxes)
                {
                    var tx = _financialService.GetTaxes().Where(t => t.Id == tax.Key).FirstOrDefault();
                    var creditSalesTaxAccount = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Cr, tx.SalesAccountId.Value, Math.Round(tax.Value, 2, MidpointRounding.ToEven));
                    glHeader.GeneralLedgerLines.Add(creditSalesTaxAccount);
                }
            }

            if (totalDiscount > 0)
            {
                var salesDiscountAccount = base.GetGeneralLedgerSetting().SalesDiscountAccount;
                var creditSalesDiscountAccount = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Dr, salesDiscountAccount.Id, Math.Round(totalDiscount, 2, MidpointRounding.ToEven));
                glHeader.GeneralLedgerLines.Add(creditSalesDiscountAccount);
            }

            if (salesInvoice.ShippingHandlingCharge > 0)
            {
                var shippingHandlingAccount = base.GetGeneralLedgerSetting().ShippingChargeAccount;
                var creditShippingHandlingAccount = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Cr, shippingHandlingAccount.Id, Math.Round(salesInvoice.ShippingHandlingCharge, 2, MidpointRounding.ToEven));
                glHeader.GeneralLedgerLines.Add(creditShippingHandlingAccount);
            }

            if (_financialService.ValidateGeneralLedgerEntry(glHeader))
            {
                salesInvoice.GeneralLedgerHeader = glHeader;

                System.Linq.Expressions.Expression<Func<SalesInvoiceHeader, object>>[] includeProperties = {
                    p => p.GeneralLedgerHeader,
                    p => p.GeneralLedgerHeader.GeneralLedgerLines,
                    p => p.SalesInvoiceLines,
                    p => p.SalesInvoiceLines.Select(p2 => p2.InventoryControlJournal)
                };                
                var dbObject = _salesInvoiceRepo.GetAllIncluding(includeProperties).Where(o => o.Id == salesInvoice.Id).FirstOrDefault();
                #region UPDATE
                if (dbObject != null)
                {
                    dbObject.No = salesInvoice.No;
                    dbObject.Description = salesInvoice.Description;
                    dbObject.Date = salesInvoice.Date;
                    dbObject.CustomerId = salesInvoice.CustomerId;
                    dbObject.ShippingHandlingCharge = salesInvoice.ShippingHandlingCharge;
                    dbObject.IsActive = salesInvoice.IsActive;

                    dbObject.GeneralLedgerHeader.Date = salesInvoice.Date;
                    dbObject.GeneralLedgerHeader.Description = salesInvoice.Description;
                    var GLLinesToUpdateIds = dbObject.GeneralLedgerHeader.GeneralLedgerLines.Select(c => c.Id).ToList();
                    foreach (var generalLedgerLine in salesInvoice.GeneralLedgerHeader.GeneralLedgerLines)
                    {
                        var existinggeneralLedgerLine = dbObject.GeneralLedgerHeader.GeneralLedgerLines.FirstOrDefault(c => c.AccountId == generalLedgerLine.AccountId);
                        if (existinggeneralLedgerLine != null)
                        {
                            existinggeneralLedgerLine.AccountId = generalLedgerLine.AccountId;
                            existinggeneralLedgerLine.Amount = generalLedgerLine.Amount;
                            existinggeneralLedgerLine.DrCr = generalLedgerLine.DrCr;
                            existinggeneralLedgerLine.IsActive = generalLedgerLine.IsActive;
                            GLLinesToUpdateIds.Remove(existinggeneralLedgerLine.Id);
                        }
                        else
                        {
                            dbObject.GeneralLedgerHeader.GeneralLedgerLines.Add(new GeneralLedgerLine()
                            {
                                DrCr = generalLedgerLine.DrCr,
                                AccountId = generalLedgerLine.AccountId,
                                Amount = generalLedgerLine.Amount
                            });
                        }
                    }
                    foreach (var generalLedgerLine in dbObject.GeneralLedgerHeader.GeneralLedgerLines.Where(c => GLLinesToUpdateIds.Contains(c.Id)))
                    {
                        generalLedgerLine.Deleted = true;
                    }
                    var salesInvoiceLinesToUpdateIds = dbObject.SalesInvoiceLines.Select(c => c.Id).ToList();
                    foreach (var salesInvoiceLine in dbObject.SalesInvoiceLines)
                    {
                        var existingSalesInvoiceLine = dbObject.SalesInvoiceLines.FirstOrDefault(c => c.Id == salesInvoiceLine.Id);
                        if (existingSalesInvoiceLine != null)
                        {
                            existingSalesInvoiceLine.ItemId = salesInvoiceLine.ItemId;
                            existingSalesInvoiceLine.MeasurementId = salesInvoiceLine.MeasurementId;
                            existingSalesInvoiceLine.TaxId = salesInvoiceLine.TaxId;
                            existingSalesInvoiceLine.Quantity = salesInvoiceLine.Quantity;
                            existingSalesInvoiceLine.Discount = salesInvoiceLine.Discount;
                            existingSalesInvoiceLine.Amount = salesInvoiceLine.Amount;
                            existingSalesInvoiceLine.TaxAmount = salesInvoiceLine.TaxAmount;
                            existingSalesInvoiceLine.DiscountAmount = salesInvoiceLine.DiscountAmount;
                            existingSalesInvoiceLine.IsActive = salesInvoiceLine.IsActive;
                            if (salesInvoiceLine.InventoryControlJournal != null)
                            {
                                if (existingSalesInvoiceLine.InventoryControlJournal != null)
                                {
                                    existingSalesInvoiceLine.InventoryControlJournal.MeasurementId = salesInvoiceLine.InventoryControlJournal.MeasurementId;
                                    existingSalesInvoiceLine.InventoryControlJournal.OUTQty = salesInvoiceLine.InventoryControlJournal.OUTQty;
                                    existingSalesInvoiceLine.InventoryControlJournal.TotalCost = salesInvoiceLine.InventoryControlJournal.TotalCost;
                                    existingSalesInvoiceLine.InventoryControlJournal.TotalAmount = salesInvoiceLine.InventoryControlJournal.TotalAmount;
                                }
                                else
                                {
                                    existingSalesInvoiceLine.InventoryControlJournal = salesInvoiceLine.InventoryControlJournal;
                                }
                            }
                            else
                            {
                                if (existingSalesInvoiceLine.InventoryControlJournal != null)
                                {
                                    existingSalesInvoiceLine.InventoryControlJournal.Deleted = true;
                                    existingSalesInvoiceLine.InventoryControlJournalId = -1;
                                }
                            }
                            salesInvoiceLinesToUpdateIds.Remove(existingSalesInvoiceLine.Id);
                        }
                        else
                        {
                            dbObject.SalesInvoiceLines.Add(salesInvoiceLine);
                        }
                    }
                    foreach (var salesInvoiceLine in dbObject.SalesInvoiceLines.Where(c => salesInvoiceLinesToUpdateIds.Contains(c.Id)))
                    {
                        salesInvoiceLine.Deleted = true;
                    }
                    salesInvoice = dbObject;
                    _salesInvoiceRepo.Update(salesInvoice);
                }
                #endregion
                #region INSERT
                else
                {
                    salesInvoice.No = GetNextNumber(SequenceNumberTypes.SalesInvoice).ToString();
                    _salesInvoiceRepo.Insert(salesInvoice);
                }
                #endregion
            }
            return salesInvoice;
        }
        #endregion

        public void AddSalesOrder(SalesOrderHeader salesOrder, bool toSave)
        {
            if (string.IsNullOrEmpty(salesOrder.No))
                salesOrder.No = GetNextNumber(SequenceNumberTypes.SalesOrder).ToString();
            if (toSave)
                _salesOrderRepo.Insert(salesOrder);
        }

        public void UpdateSalesOrder(SalesOrderHeader salesOrder)
        {
            var persisted = _salesOrderRepo.GetById(salesOrder.Id);
            foreach (var persistedLine in persisted.SalesOrderLines)
            {
                bool found = false;
                foreach (var currentLine in salesOrder.SalesOrderLines)
                {
                    if (persistedLine.Id == currentLine.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (found)
                    continue;
                else
                {

                }
            }
            _salesOrderRepo.Update(salesOrder);
        }

        public void AddSalesReceipt(SalesReceiptHeader salesReceipt)
        {
            var customer = _customerRepo.GetById(salesReceipt.CustomerId);
            var glHeader = _financialService.CreateGeneralLedgerHeader(DocumentTypes.SalesReceipt, salesReceipt.Date, string.Empty);
            var debit = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Dr, salesReceipt.AccountToDebitId.Value, salesReceipt.SalesReceiptLines.Sum(i => i.AmountPaid));
            var credit = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Cr, customer.AccountsReceivableAccountId.Value, salesReceipt.SalesReceiptLines.Sum(i => i.AmountPaid));
            glHeader.GeneralLedgerLines.Add(debit);
            glHeader.GeneralLedgerLines.Add(credit);

            if (_financialService.ValidateGeneralLedgerEntry(glHeader))
            {
                salesReceipt.GeneralLedgerHeader = glHeader;

                salesReceipt.No = GetNextNumber(SequenceNumberTypes.SalesReceipt).ToString();
                _salesReceiptRepo.Insert(salesReceipt);
            }
        }

        /// <summary>
        /// Customer advances. Initial recognition. Debit to cash (asset), credit to customer advances (liability)
        /// </summary>
        /// <param name="salesReceipt"></param>
        public void AddSalesReceiptNoInvoice(SalesReceiptHeader salesReceipt)
        {
            var customer = _customerRepo.GetById(salesReceipt.CustomerId);
            var glHeader = _financialService.CreateGeneralLedgerHeader(DocumentTypes.SalesReceipt, salesReceipt.Date, string.Empty);
            var debit = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Dr, salesReceipt.AccountToDebitId.Value, salesReceipt.Amount);
            glHeader.GeneralLedgerLines.Add(debit);

            foreach (var line in salesReceipt.SalesReceiptLines)
            {
                var credit = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Cr, line.AccountToCreditId.Value, line.AmountPaid);
                glHeader.GeneralLedgerLines.Add(credit);
            }

            if (_financialService.ValidateGeneralLedgerEntry(glHeader))
            {
                salesReceipt.GeneralLedgerHeader = glHeader;

                salesReceipt.No = GetNextNumber(SequenceNumberTypes.SalesReceipt).ToString();
                _salesReceiptRepo.Insert(salesReceipt);
            }
        }

        public IQueryable<SalesReceiptHeader> GetSalesReceipts()
        {
            var query = from receipt in _salesReceiptRepo.Table
                        select receipt;
            return query;
        }

        public SalesReceiptHeader GetSalesReceiptById(int id)
        {
            return _salesReceiptRepo.GetById(id);
        }

        public void UpdateSalesReceipt(SalesReceiptHeader salesReceipt)
        {
            _salesReceiptRepo.Update(salesReceipt);
        }

        public ICollection<SalesReceiptHeader> GetCustomerReceiptsForAllocation(int customerId)
        {
            var customerReceipts = _salesReceiptRepo.Table.Where(r => r.CustomerId == customerId);
            var customerReceiptsWithNoInvoice = new HashSet<SalesReceiptHeader>();
            foreach (var receipt in customerReceipts)
            {
                //if (receipt.SalesInvoiceHeaderId == null)
                //    customerReceiptsWithNoInvoice.Add(receipt);
                customerReceiptsWithNoInvoice.Add(receipt);
            }
            return customerReceiptsWithNoInvoice;
        }

        public void SaveCustomerAllocation(CustomerAllocation allocation)
        {
            //Revenue recognition. Debit the customer advances (liability) account and credit the revenue account.
            //In case of allocation, credit the accounts receivable since sales account is already credited from invoice.
            var invoice = _salesInvoiceRepo.GetById(allocation.SalesInvoiceHeaderId);
            var receipt = _salesReceiptRepo.GetById(allocation.SalesReceiptHeaderId);

            var glHeader = _financialService.CreateGeneralLedgerHeader(BusinessCore.Domain.DocumentTypes.CustomerAllocation, allocation.Date, string.Empty);

            foreach (var line in receipt.SalesReceiptLines)
            {
                Account accountToDebit = invoice.Customer.CustomerAdvancesAccount;
                var debit = _financialService.CreateGeneralLedgerLine(BusinessCore.Domain.DrOrCrSide.Dr, accountToDebit.Id, allocation.Amount);
                glHeader.GeneralLedgerLines.Add(debit);
            }

            Account accountToCredit = invoice.Customer.AccountsReceivableAccount;
            var credit = _financialService.CreateGeneralLedgerLine(BusinessCore.Domain.DrOrCrSide.Cr, accountToCredit.Id, allocation.Amount);
            glHeader.GeneralLedgerLines.Add(credit);

            if (_financialService.ValidateGeneralLedgerEntry(glHeader))
            {
                invoice.GeneralLedgerHeader = glHeader;
                invoice.CustomerAllocations.Add(allocation);
                _salesInvoiceRepo.Update(invoice);
            }
        }

        public IQueryable<SalesDeliveryHeader> GetSalesDeliveries()
        {
            var query = from f in _salesDeliveryRepo.Table
                        select f;
            return query;
        }

        public void AddSalesDelivery(SalesDeliveryHeader salesDelivery, bool toSave)
        {
            var glHeader = _financialService.CreateGeneralLedgerHeader(DocumentTypes.SalesDelivery, salesDelivery.Date, string.Empty);
            // Debit = COGS, Credit = Inventory
            var debitAccounts = new List<KeyValuePair<int, decimal>>();
            var creditAccounts = new List<KeyValuePair<int, decimal>>();
            foreach (var line in salesDelivery.SalesDeliveryLines)
            {
                var item = _inventoryService.GetItemById(line.ItemId.Value);
                debitAccounts.Add(new KeyValuePair<int, decimal>(item.CostOfGoodsSoldAccountId.Value, item.Cost.Value * line.Quantity));
                creditAccounts.Add(new KeyValuePair<int, decimal>(item.InventoryAccountId.Value, item.Cost.Value * line.Quantity));
            }
            var groupedDebitAccounts = (from kvp in debitAccounts
                                        group kvp by kvp.Key into g
                                        select new KeyValuePair<int, decimal>(g.Key, g.Sum(e => e.Value)));
            var groupedCreditAccounts = (from kvp in creditAccounts
                                         group kvp by kvp.Key into g
                                         select new KeyValuePair<int, decimal>(g.Key, g.Sum(e => e.Value)));
            foreach (var account in groupedDebitAccounts)
            {
                glHeader.GeneralLedgerLines.Add(_financialService.CreateGeneralLedgerLine(DrOrCrSide.Dr, account.Key, account.Value));
            }
            foreach (var account in groupedCreditAccounts)
            {
                glHeader.GeneralLedgerLines.Add(_financialService.CreateGeneralLedgerLine(DrOrCrSide.Cr, account.Key, account.Value));
            }

            if (_financialService.ValidateGeneralLedgerEntry(glHeader))
            {
                salesDelivery.GeneralLedgerHeader = glHeader;

                salesDelivery.No = GetNextNumber(SequenceNumberTypes.SalesDelivery).ToString();

                if (!salesDelivery.SalesOrderHeaderId.HasValue)
                {
                    var salesOrder = new SalesOrderHeader()
                    {
                        CustomerId = salesDelivery.CustomerId,
                        PaymentTermId = salesDelivery.PaymentTermId,
                        Date = salesDelivery.Date,
                        No = GetNextNumber(SequenceNumberTypes.SalesOrder).ToString(),
                    };

                    foreach (var line in salesDelivery.SalesDeliveryLines)
                    {
                        var item = _inventoryService.GetItemById(line.ItemId.Value);
                        salesOrder.SalesOrderLines.Add(new SalesOrderLine()
                        {
                            ItemId = item.Id,
                            MeasurementId = line.MeasurementId.Value,
                            Quantity = line.Quantity,
                            Amount = item.Price.Value,
                        });
                    }
                    AddSalesOrder(salesOrder, false);
                    salesDelivery.SalesOrderHeader = salesOrder;
                }

                if (toSave)
                    _salesDeliveryRepo.Insert(salesDelivery);
            }
        }

        public IQueryable<SalesOrderHeader> GetSalesOrders()
        {
            var salesOrders = _salesOrderRepo.GetAllIncluding(c => c.Customer,
                pt => pt.PaymentTerm,
                lines => lines.SalesOrderLines);

            foreach (var salesOrder in salesOrders)
            {
                salesOrder.Customer.Party = GetCustomerById(salesOrder.CustomerId.Value).Party;
            }

            return salesOrders;
        }

        public SalesOrderHeader GetSalesOrderById(int id)
        {
            var salesOrder = _salesOrderRepo.GetAllIncluding(lines => lines.SalesOrderLines,
                c => c.Customer,
                p => p.PaymentTerm)
                .Where(o => o.Id == id).FirstOrDefault()
                ;

            foreach (var line in salesOrder.SalesOrderLines)
            {
                line.Item = _itemRepo.GetById(line.ItemId);
                line.Measurement = _measurementRepo.GetById(line.MeasurementId);
            }

            return salesOrder;
        }

        public SalesDeliveryHeader GetSalesDeliveryById(int id)
        {
            return _salesDeliveryRepo.GetById(id);
        }

        public IQueryable<Contact> GetContacts()
        {
            var query = from f in _contactRepo.Table
                        select f;
            return query;
        }

        public int SaveContact(Contact contact)
        {
            _contactRepo.Insert(contact);
            return contact.Id;
        }

        public ICollection<SalesInvoiceHeader> GetSalesInvoicesByCustomerId(int customerId, SalesInvoiceStatus status)
        {
            var query = from invoice in _salesInvoiceRepo.Table
                        where invoice.CustomerId == customerId && invoice.Status == status
                        select invoice;
            return query.ToList();
        }

        public ICollection<CustomerAllocation> GetCustomerAllocations(int customerId)
        {
            return null;
        }
    }
}
