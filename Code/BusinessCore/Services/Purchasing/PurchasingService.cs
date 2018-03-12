using BusinessCore.Data;
using BusinessCore.Domain;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using BusinessCore.Services.Financial;
using BusinessCore.Services.Inventory;
using System.Collections.Generic;
using System.Linq;
using System;
using BusinessCore.Services.Security;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using BusinessCore.Domain.TaxSystem;

namespace BusinessCore.Services.Purchasing
{
    public partial class PurchasingService : BaseService, IPurchasingService
    {
        private readonly IFinancialService _financialService;
        private readonly IInventoryService _inventoryService;

        private readonly IRepository<PurchaseOrderHeader> _purchaseOrderRepo;
        private readonly IRepository<PurchaseInvoiceHeader> _purchaseInvoiceRepo;
        private readonly IRepository<PurchaseReceiptHeader> _purchaseReceiptRepo;
        private readonly IRepository<Vendor> _vendorRepo;
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<Measurement> _measurementRepo;
        private readonly IRepository<SequenceNumber> _sequenceNumberRepo;
        private readonly IRepository<VendorPayment> _vendorPaymentRepo;
        private readonly IRepository<GeneralLedgerSetting> _generalLedgerSettingRepo;
        private readonly IRepository<PaymentTerm> _paymentTermRepo;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IRepository<Tax> _taxRepo;
        private readonly IRepository<TaxGroupTax> _taxGroupTaxRepo;
        private readonly IRepository<TaxGroup> _taxGroupRepo;

        public PurchasingService(IFinancialService financialService,
            IInventoryService inventoryService,
            IRepository<PurchaseOrderHeader> purchaseOrderRepo,
            IRepository<PurchaseInvoiceHeader> purchaseInvoiceRepo,
            IRepository<PurchaseReceiptHeader> purchaseReceiptRepo,
            IRepository<Vendor> vendorRepo,
            IRepository<Account> accountRepo,
            IRepository<Item> itemRepo,
            IRepository<Measurement> measurementRepo,
            IRepository<SequenceNumber> sequenceNumberRepo,
            IRepository<VendorPayment> vendorPaymentRepo,
            IRepository<GeneralLedgerSetting> generalLedgerSettingRepo,
            IRepository<PaymentTerm> paymentTermRepo,
            IRepository<Bank> bankRepo,
            IRepository<Tax> taxRepo,
            IRepository<TaxGroupTax> taxGroupTaxRepo,
            IRepository<TaxGroup> taxGroupRepo
            )
            : base(sequenceNumberRepo, generalLedgerSettingRepo, paymentTermRepo, bankRepo)
        {
            _financialService = financialService;
            _inventoryService = inventoryService;

            _purchaseOrderRepo = purchaseOrderRepo;
            _purchaseInvoiceRepo = purchaseInvoiceRepo;
            _purchaseReceiptRepo = purchaseReceiptRepo;
            _vendorRepo = vendorRepo;
            _accountRepo = accountRepo;
            _itemRepo = itemRepo;
            _measurementRepo = measurementRepo;
            _sequenceNumberRepo = sequenceNumberRepo;
            _vendorPaymentRepo = vendorPaymentRepo;
            _generalLedgerSettingRepo = generalLedgerSettingRepo;
            _paymentTermRepo = paymentTermRepo;
            _bankRepo = bankRepo;
            _taxRepo = taxRepo;
            _taxGroupTaxRepo = taxGroupTaxRepo;
            _taxGroupRepo = taxGroupRepo;
        }

        public void SavePurchaseInvoice(PurchaseInvoiceHeader purchaseIvoice)
        {
            var invoice = GetPurchaseInvoiceById(purchaseIvoice.Id);
            if (invoice != null)
            {
                invoice.Date = purchaseIvoice.Date;
                invoice.No = purchaseIvoice.No;
                invoice.VendorId = purchaseIvoice.VendorId;
                invoice.Description = purchaseIvoice.Description;
                // update invoice lines from old invoice
                foreach (var line in invoice.PurchaseInvoiceLines)
                {
                    var singleLine = purchaseIvoice.PurchaseInvoiceLines.Where(x => x.Id == line.Id).FirstOrDefault();
                    if (singleLine != null)
                    {
                        line.ItemId = singleLine.ItemId;
                        line.MeasurementId = singleLine.MeasurementId;
                        line.Quantity = singleLine.Quantity;
                        line.Cost = singleLine.Cost;
                        line.Discount = singleLine.Discount;
                        line.Amount = singleLine.Amount;
                        // TODO: tax is not mapped coming from front end with amount
                    }
                    else
                    {
                        line.Deleted = true;
                    }
                }
                foreach (var line in purchaseIvoice.PurchaseInvoiceLines)
                {
                    var singleLine = invoice.PurchaseInvoiceLines.Where(x => x.Id == line.Id).FirstOrDefault();
                    if (singleLine == null)
                    {
                        line.Id = 0;
                        invoice.PurchaseInvoiceLines.Add(line);
                    }
                }
                // end update
                var glHeader = GenerateGLHeader(invoice.PurchaseInvoiceLines, invoice);
                if (_financialService.ValidateGeneralLedgerEntry(glHeader))
                {
                    purchaseIvoice.GeneralLedgerHeader = glHeader;

                    purchaseIvoice.No = GetNextNumber(SequenceNumberTypes.PurchaseInvoice).ToString();
                    _purchaseInvoiceRepo.Update(purchaseIvoice);
                }
            }
            else
            {

                var glHeader = GenerateGLHeader(purchaseIvoice.PurchaseInvoiceLines, purchaseIvoice);
                if (_financialService.ValidateGeneralLedgerEntry(glHeader))
                {
                    purchaseIvoice.GeneralLedgerHeader = glHeader;

                    purchaseIvoice.No = GetNextNumber(SequenceNumberTypes.PurchaseInvoice).ToString();
                    _purchaseInvoiceRepo.Insert(purchaseIvoice);

                    // TODO: Look for other way to update the purchase order's invoice header id field so that it shall be in a single transaction along with purchase invoice saving
                    //var purchOrder = _purchaseOrderRepo.GetById(purchaseOrderId.Value);
                    // purchOrder.PurchaseInvoiceHeaderId = purchaseIvoice.Id;
                    //_purchaseOrderRepo.Update(purchOrder);
                }
            }
        }

        public dynamic GenerateGLHeader(ICollection<PurchaseInvoiceLine> InvoiceLines, PurchaseInvoiceHeader purchaseIvoice)
        {
            var glHeader = _financialService.CreateGeneralLedgerHeader(DocumentTypes.PurchaseInvoice, purchaseIvoice.Date, purchaseIvoice.Description);

            decimal totalTaxAmount = 0, totalAmount = 0, totalDiscount = 0;
            var taxes = new List<KeyValuePair<int, decimal>>();

            foreach (var line in InvoiceLines)
            {
                var lineTaxes = _financialService.ComputeInputTax(purchaseIvoice.VendorId.Value, line.ItemId, line.Quantity, line.Cost.Value, decimal.Zero);

                var lineAmount = line.Quantity * line.Cost;

                var totalLineAmount = lineAmount + lineTaxes.Sum(t => t.Value);

                totalAmount += (decimal)totalLineAmount;

                foreach (var t in lineTaxes)
                    taxes.Add(t);
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

                totalTaxAmount = taxes.Sum(t => t.Value);

                foreach (var tax in groupedTaxes)
                {
                    var tx = _financialService.GetTaxes().Where(t => t.Id == tax.Key).FirstOrDefault();
                    var debitPurchaseTaxAccount = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Dr, tx.PurchasingAccountId.Value, tax.Value);
                    glHeader.GeneralLedgerLines.Add(debitPurchaseTaxAccount);

                }
            }

            if (totalDiscount > 0)
            {

            }

            Vendor vendor = _vendorRepo.GetById(purchaseIvoice.VendorId.Value);
            var creditVendorAccount = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Cr, vendor.AccountsPayableAccountId.Value, totalAmount);
            glHeader.GeneralLedgerLines.Add(creditVendorAccount);

            var debitGRNClearingAccount = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Dr, GetGeneralLedgerSetting().GoodsReceiptNoteClearingAccountId.Value, totalAmount - totalTaxAmount);
            glHeader.GeneralLedgerLines.Add(debitGRNClearingAccount);

            return glHeader;
        }

        public void AddPurchaseOrder(PurchaseOrderHeader purchaseOrder, bool toSave)
        {
            purchaseOrder.No = GetNextNumber(SequenceNumberTypes.PurchaseOrder).ToString();

            if (toSave)
                _purchaseOrderRepo.Insert(purchaseOrder);
        }

        public void AddPurchaseOrderReceipt(PurchaseReceiptHeader purchaseOrderReceipt)
        {
            var glLines = new List<GeneralLedgerLine>();
            decimal totalAmount = purchaseOrderReceipt.PurchaseReceiptLines.Sum(d => d.Amount);
            decimal taxAmount = purchaseOrderReceipt.GetTotalTax();
            decimal totalDiscount = 0;

            foreach (var lineItem in purchaseOrderReceipt.PurchaseReceiptLines)
            {
                var item = _itemRepo.GetById(lineItem.ItemId);
                decimal lineItemTotalAmountAfterTax = lineItem.Amount - lineItem.LineTaxAmount;

                GeneralLedgerLine debitInventory = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Dr, item.InventoryAccountId ?? 0, lineItemTotalAmountAfterTax);
                glLines.Add(debitInventory);

                GeneralLedgerLine creditGRNClearingAccount = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Cr, GetGeneralLedgerSetting().GoodsReceiptNoteClearingAccountId.Value, lineItemTotalAmountAfterTax);
                glLines.Add(creditGRNClearingAccount);

                lineItem.InventoryControlJournal = _inventoryService.CreateInventoryControlJournal(lineItem.ItemId,
                    lineItem.MeasurementId,
                    DocumentTypes.PurchaseReceipt,
                    lineItem.ReceivedQuantity,
                    null,
                    lineItem.ReceivedQuantity * item.Cost,
                    null);
            }

            if (taxAmount > 0)
            {
            }

            if (totalDiscount > 0)
            {
            }

            GeneralLedgerHeader glHeader = _financialService.CreateGeneralLedgerHeader(DocumentTypes.PurchaseReceipt, purchaseOrderReceipt.Date, string.Empty);
            glHeader.GeneralLedgerLines = glLines;

            if (_financialService.ValidateGeneralLedgerEntry(glHeader))
            {
                purchaseOrderReceipt.GeneralLedgerHeader = glHeader;

                purchaseOrderReceipt.No = GetNextNumber(SequenceNumberTypes.PurchaseReceipt).ToString();
                _purchaseReceiptRepo.Insert(purchaseOrderReceipt);
            }
        }

        public IQueryable<Measurement> GetItems()
        {
            var query = _measurementRepo.Table;
            return query;
        }
        public IQueryable<Tax> GetTaxes()
        {
            var query = _taxRepo.Table;
            return query;
        }
        public IQueryable<TaxGroup> GetTaxGroup()
        {
            var query= _taxGroupRepo.Table;
            return query;
        }
        public  IQueryable<TaxGroupTax> GetTaxGroupTax()
        {
            return _taxGroupTaxRepo.Table;
        }
        public IQueryable<PaymentTerm> GetPaymentTerms()
        {
            var query = _paymentTermRepo.Table;
            return query;
        }
        public IQueryable<Vendor> GetVendors()
        {
            var query = _vendorRepo.Table
                .Include(p => p.Party)
                .Include(p => p.PaymentTerm)
                .Include(p => p.TaxGroup)
                ;
            return query;
        }

        void IPurchasingService.UpdateVendor(Vendor vendor)
        {
            var dbObject = GetVendorById(vendor.Id);
            if (dbObject != null)
            {
                dbObject.No = vendor.No;
                dbObject.Party.Name = vendor.Party.Name;
                dbObject.Party.Email = vendor.Party.Email;
                dbObject.Party.Website = vendor.Party.Website;
                _vendorRepo.Update(dbObject);
            }
        }

        public Vendor GetVendorById(int id)
        {
            return _vendorRepo.Table
                .Include(p => p.Party)
                .ThenInclude(p => p.Contacts)
                .Include(p => p.PaymentTerm)
                .Include(p => p.TaxGroup)
                .FirstOrDefault(o => o.Id == id);
        }

        void IPurchasingService.DeleteVendor(int VendorId)
        {
            var dbObject = GetVendorById(VendorId);
            if (dbObject != null)
            {
                dbObject.Deleted = true;
                _vendorRepo.Update(dbObject);
            }
        }


        public IQueryable<PurchaseOrderHeader> GetPurchaseOrders()
        {
            var query = _purchaseOrderRepo.Table;

            return query;
        }

        public PurchaseOrderHeader GetPurchaseOrderById(int id)
        {
            return _purchaseOrderRepo.GetById(id);
        }

        public PurchaseReceiptHeader GetPurchaseReceiptById(int id)
        {
            return _purchaseReceiptRepo.GetById(id);
        }
        public Vendor SaveVendor(Vendor vendor)
        {
            var dbObject = GetVendorById(vendor.Id);
            #region UPDATE
            if (dbObject != null)
            {
                dbObject.No = vendor.No;
                dbObject.TaxGroupId = vendor.TaxGroupId;
                dbObject.PaymentTermId = vendor.PaymentTermId;
                dbObject.TaxGroupId = vendor.TaxGroupId;

                dbObject.AccountsPayableAccountId = vendor.AccountsPayableAccountId;
                dbObject.PurchaseAccountId = vendor.PurchaseAccountId;
                dbObject.PurchaseDiscountAccountId = vendor.PurchaseDiscountAccountId;

                dbObject.IsActive = vendor.IsActive;

                dbObject.Party.Address = vendor.Party.Address;
                dbObject.Party.Email = vendor.Party.Email;
                dbObject.Party.Name = vendor.Party.Name;
                dbObject.Party.Phone = vendor.Party.Phone;
                dbObject.Party.Fax = vendor.Party.Fax;
                dbObject.Party.IsActive = vendor.Party.IsActive;

                var contactsToUpdateIds = dbObject.Party.Contacts.Select(c => c.Id).ToList();
                foreach (var contact in vendor.Party.Contacts)
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
                            ContactType = ContactTypes.Vendor,
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


                _vendorRepo.Update(dbObject);
                vendor = dbObject;
            }
            #endregion
            #region INSERT
            else
            {
                vendor.No = GetNextNumber(SequenceNumberTypes.Vendor).ToString();

                vendor.AccountsPayableAccountId = _accountRepo.Table.Where(a => a.AccountCode == AccountCodes.AccountsPayable_20110).FirstOrDefault().Id;
                vendor.PurchaseAccountId = _accountRepo.Table.Where(a => a.AccountCode == AccountCodes.Purchase_50200).FirstOrDefault().Id;
                vendor.PurchaseDiscountAccountId = _accountRepo.Table.Where(a => a.AccountCode == AccountCodes.PurchaseDiscount_50400).FirstOrDefault().Id;

                vendor.Party.PartyType = PartyTypes.Customer;
                foreach (var contact in vendor.Party.Contacts)
                {
                    contact.Party = vendor.Party;
                    contact.ContactType = ContactTypes.Company;
                }

                _vendorRepo.Insert(vendor);
            }
            #endregion
            return vendor;
        }
        public IQueryable<PurchaseInvoiceHeader> GetPurchaseInvoices()
        {
            var query = from purchInvoice in _purchaseInvoiceRepo.Table
                        select purchInvoice;
            return query;
        }

        public PurchaseInvoiceHeader GetPurchaseInvoiceById(int id)
        {
            var query = _purchaseInvoiceRepo.Table
                        .Include(p => p.PurchaseInvoiceLines)
                        .Include(p => p.Vendor)
                        .Include(p => p.GeneralLedgerHeader)
                        .Where(invoice => invoice.Id == id);

            return query.FirstOrDefault();
        }

        public void SavePayment(int invoiceId, int vendorId, int accountId, decimal amount, DateTime date)
        {
            var payment = new VendorPayment()
            {
                VendorId = vendorId,
                PurchaseInvoiceHeaderId = invoiceId,
                Date = date,
                Amount = amount,
            };
            var vendor = GetVendorById(vendorId);
            var debit = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Dr, vendor.AccountsPayableAccountId.Value, amount);
            var credit = _financialService.CreateGeneralLedgerLine(DrOrCrSide.Cr, accountId, amount);
            var glHeader = _financialService.CreateGeneralLedgerHeader(DocumentTypes.PurchaseInvoicePayment, date, string.Empty);
            glHeader.GeneralLedgerLines.Add(debit);
            glHeader.GeneralLedgerLines.Add(credit);

            if (_financialService.ValidateGeneralLedgerEntry(glHeader))
            {
                payment.GeneralLedgerHeader = glHeader;

                payment.No = GetNextNumber(SequenceNumberTypes.VendorPayment).ToString();
                _vendorPaymentRepo.Insert(payment);
            }
        }

       
    }
}

