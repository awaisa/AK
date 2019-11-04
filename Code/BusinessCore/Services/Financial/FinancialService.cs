﻿using BusinessCore.Data;
using BusinessCore.Domain;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.Sales;
using BusinessCore.Domain.TaxSystem;
using BusinessCore.Services.Inventory;
using BusinessCore.Services.TaxSystem;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessCore.Services.Financial
{
    public partial class FinancialService : BaseService, IFinancialService
    {
        private readonly IInventoryService _inventoryService;
        private readonly ITaxService _taxService;

        private readonly IRepository<GeneralLedgerHeader> _generalLedgerRepository;
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<Tax> _taxRepo;
        private readonly IRepository<JournalEntryHeader> _journalEntryRepo;
        private readonly IRepository<GeneralLedgerLine> _generalLedgerLineRepository;
        private readonly IRepository<FinancialYear> _fiscalYearRepo;
        private readonly IRepository<TaxGroup> _taxGroupRepo;
        private readonly IRepository<ItemTaxGroup> _itemTaxGroupRepo;
        private readonly IRepository<PaymentTerm> _paymentTermRepo;
        private readonly IRepository<Bank> _bankRepo;
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<GeneralLedgerSetting> _glSettingRepo;
        private readonly IRepository<MainContraAccount> _maincontraAccount;
        private readonly IRepository<Customer> _customerRepo;
        private readonly IRepository<Vendor> _vendorRepo;
        private readonly IRepository<GeneralLedgerSetting> _generalLedgerSettingRepo;

        public FinancialService(IInventoryService inventoryService, 
            ITaxService taxService,
            IRepository<GeneralLedgerHeader> generalLedgerRepository,
            IRepository<GeneralLedgerLine> generalLedgerLineRepository,
            IRepository<Account> accountRepo,
            IRepository<Tax> taxRepo,
            IRepository<JournalEntryHeader> journalEntryRepo,
            IRepository<FinancialYear> fiscalYearRepo,
            IRepository<TaxGroup> taxGroupRepo,
            IRepository<ItemTaxGroup> itemTaxGroupRepo,
            IRepository<PaymentTerm> paymentTermRepo,
            IRepository<Bank> bankRepo,
            IRepository<Item> itemRepo,
            IRepository<GeneralLedgerSetting> glSettingRepo,
            IRepository<MainContraAccount> maincontraAccount = null,
            IRepository<Customer> customerRepo = null,
            IRepository<Vendor> vendorRepo = null, 
            IRepository<GeneralLedgerSetting> generalLedgerSettingRepo = null
            )
            :base(null, null, paymentTermRepo, bankRepo)
        {
            _inventoryService = inventoryService;
            _taxService = taxService;

            _generalLedgerRepository = generalLedgerRepository;
            _accountRepo = accountRepo;
            _taxRepo = taxRepo;
            _journalEntryRepo = journalEntryRepo;
            _generalLedgerLineRepository = generalLedgerLineRepository;
            _fiscalYearRepo = fiscalYearRepo;
            _taxGroupRepo = taxGroupRepo;
            _itemTaxGroupRepo = itemTaxGroupRepo;
            _paymentTermRepo = paymentTermRepo;
            _bankRepo = bankRepo;
            _itemRepo = itemRepo;
            _glSettingRepo = glSettingRepo;
            _maincontraAccount = maincontraAccount;
            _customerRepo = customerRepo;
            _vendorRepo = vendorRepo;
            _generalLedgerSettingRepo = generalLedgerSettingRepo;
        }

        public FinancialYear CurrentFiscalYear()
        {
            var query = (from fy in _fiscalYearRepo.Table
                        where fy.IsActive == true
                        select fy).FirstOrDefault();

            return query;
        }

        public GeneralLedgerHeader CreateGeneralLedgerHeader(DocumentTypes documentType, DateTime date, string description)
        {
            var entry = new GeneralLedgerHeader()
            {
                DocumentType = documentType,
                Date = date,
                Description = description,
            };
            return entry;
        }

        public GeneralLedgerLine CreateGeneralLedgerLine(DrOrCrSide DrCr, int accountId, decimal amount)
        {
            var line = new GeneralLedgerLine()
            {
                DrCr = DrCr,
                AccountId = accountId,
                Amount = amount,
            };
            return line;
        }

        public IQueryable<Account> GetAccounts()
        {
            return _accountRepo.Table.Include(p => p.AccountClass);
        }

        public Account GetAccount(int id)
        {
            return _accountRepo.Table.Where(a => a.Id == id).FirstOrDefault();
        }

        public Account GetAccountByAccountCode(string accountcode)
        {
            return _accountRepo.Table.Where(a => a.AccountCode == accountcode).FirstOrDefault();
        }

        public IEnumerable<Tax> GetTaxes()
        {
            var query = from f in _taxRepo.Table
                        select f;
            return query.AsEnumerable();
        }

        public IEnumerable<ItemTaxGroup> GetItemTaxGroups()
        {
            var query = from f in _itemTaxGroupRepo.Table
                        select f;
            return query;
        }

        public IEnumerable<TaxGroup> GetTaxGroups()
        {
            var query = from f in _taxGroupRepo.Table
                        select f;
            return query;
        }

        public void SaveGeneralLedgerEntry(GeneralLedgerHeader entry)
        {
            if (ValidateGeneralLedgerEntry(entry))
            {
                try
                {
                    _generalLedgerRepository.Insert(entry);
                }
                catch { }
            }
        }

        public bool ValidateGeneralLedgerEntry(GeneralLedgerHeader glEntry)
        {
            if (!FinancialHelper.DrCrEqualityValidated(glEntry))
                throw new InvalidOperationException("Debit/Credit are not equal.");

            if (!FinancialHelper.NoLineAmountIsEqualToZero(glEntry))
                throw new InvalidOperationException("One or more line(s) amount is zero.");

            var fiscalYear = CurrentFiscalYear();
            if (fiscalYear == null 
                || !FinancialHelper.InRange(glEntry.Date, fiscalYear.StartDate, fiscalYear.EndDate)
                || !FinancialHelper.InRange(DateTime.Now, fiscalYear.StartDate, fiscalYear.EndDate))
                throw new InvalidOperationException("Date is out of range. Must within financial year.");

            //var duplicateAccounts = glEntry.GeneralLedgerLines.GroupBy(gl => gl.AccountId).Where(gl => gl.Count() > 1);
            //if (duplicateAccounts.Count() > 0)
            //    throw new InvalidOperationException("Duplicate account id in a collection.");

            foreach (var account in glEntry.GeneralLedgerLines)
                if (!_accountRepo.GetById(account.AccountId).CanPost())
                    throw new InvalidOperationException("One of the account is not valid for posting");

            //if(!glEntry.ValidateAccountingEquation())
            //    throw new InvalidOperationException("One of the account not equal.");

            return true;
        }

        public IQueryable<JournalEntryHeader> GetJournalEntries()
        {
            var query = from je in _journalEntryRepo.Table
                        select je;
            return query;
        }
        public JournalEntryHeader GetJournalEntryById(int id)
        {
            var journalEntryRepo = _journalEntryRepo.Table
                .Include(p => p.Party)
                .Include(p => p.Party.Contacts)
                .Include(p => p.JournalEntryLines)
                .Where(c => c.Id == id).FirstOrDefault();

            return journalEntryRepo;
        }
        public JournalEntryHeader SaveJournalEntry(JournalEntryHeader journalEntry)
        {
            var dbObject = GetJournalEntry(journalEntry.Id);
            #region UPDATE
            if (dbObject != null)
            {
                dbObject.Date = journalEntry.Date;
                dbObject.Memo = journalEntry.Memo;
                dbObject.ReferenceNo = journalEntry.ReferenceNo;
                dbObject.IsActive = journalEntry.IsActive;
                
                var toUpdateIds = dbObject.JournalEntryLines.Select(c => c.Id).ToList();
                foreach (var lineItem in journalEntry.JournalEntryLines)
                {
                    var existingLineItem = dbObject.JournalEntryLines.FirstOrDefault(c => c.Id == lineItem.Id);
                    if (existingLineItem != null)
                    {
                        existingLineItem.AccountId = lineItem.AccountId;
                        existingLineItem.DrCr = lineItem.DrCr;
                        existingLineItem.Memo = lineItem.Memo;
                        existingLineItem.Amount = lineItem.Amount;
                        existingLineItem.IsActive = lineItem.IsActive;
                        toUpdateIds.Remove(existingLineItem.Id);
                    }
                    else
                    {
                        dbObject.JournalEntryLines.Add(new JournalEntryLine()
                        {
                            AccountId = lineItem.AccountId,
                            DrCr = lineItem.DrCr,
                            Memo = lineItem.Memo,
                            Amount = lineItem.Amount,
                        });
                    }
                }

                _journalEntryRepo.Update(dbObject);
                journalEntry = dbObject;
            }
            #endregion
            #region INSERT
            else
            {
                _journalEntryRepo.Insert(journalEntry);
            }
            #endregion
            return journalEntry;
        }

        public void AddJournalEntry(JournalEntryHeader journalEntry)
        {
            journalEntry.Posted = false;

            _journalEntryRepo.Insert(journalEntry);
        }

        public ICollection<TrialBalance> TrialBalance(DateTime? from = default(DateTime?), DateTime? to = default(DateTime?))
        {            
            var allDr = (from dr in _generalLedgerLineRepository.Table.AsEnumerable()
                         where dr.DrCr == DrOrCrSide.Dr
                         //&& IsDateBetweenFinancialYearStartDateAndEndDate(dr.GLHeader.Date)
                         group dr by new { dr.AccountId, dr.Account.AccountCode, dr.Account.AccountName, dr.Amount } into tb
                         select new
                         {
                             AccountId = tb.Key.AccountId,
                             AccountCode = tb.Key.AccountCode,
                             AccountName = tb.Key.AccountName,
                             Debit = tb.Sum(d => d.Amount),
                         });

            var allCr = (from cr in _generalLedgerLineRepository.Table.AsEnumerable()
                         where cr.DrCr == DrOrCrSide.Cr
                         //&& IsDateBetweenFinancialYearStartDateAndEndDate(cr.GLHeader.Date)
                         group cr by new { cr.AccountId, cr.Account.AccountCode, cr.Account.AccountName, cr.Amount } into tb
                         select new
                         {
                             AccountId = tb.Key.AccountId,
                             AccountCode = tb.Key.AccountCode,
                             AccountName = tb.Key.AccountName,
                             Credit = tb.Sum(c => c.Amount),
                         });

            var allDrcr = (from x in allDr
                           select new TrialBalance
                           {
                               AccountId = x.AccountId,
                               AccountCode = x.AccountCode,
                               AccountName = x.AccountName,
                               Debit = x.Debit,
                               Credit = (decimal)0,
                           }
                          ).Concat(from y in allCr
                                   select new TrialBalance
                                   {
                                       AccountId = y.AccountId,
                                       AccountCode = y.AccountCode,
                                       AccountName = y.AccountName,
                                       Debit = (decimal)0,
                                       Credit = y.Credit,
                                   });

            var sortedList = allDrcr
                .OrderBy(tb => tb.AccountCode)
                .ToList()
                .Reverse<TrialBalance>();

            var accounts = sortedList.ToList().GroupBy(a => a.AccountCode)
                .Select(tb => new TrialBalance
                {
                    AccountId = tb.First().AccountId,
                    AccountCode = tb.First().AccountCode,
                    AccountName = tb.First().AccountName,
                    Credit = tb.Sum(x => x.Credit),
                    Debit = tb.Sum(y => y.Debit)
                }).ToList();
            return accounts;
        }

        public ICollection<BalanceSheet> BalanceSheet(DateTime? from = default(DateTime?), DateTime? to = default(DateTime?))
        {
            var assets = from a in _accountRepo.Table
                         where a.AccountClassId == 1 && a.ParentAccountId != null && !a.IsContraAccount
                         select a;
            var liabilities = from a in _accountRepo.Table
                              where a.AccountClassId == 2 && a.ParentAccountId != null && !a.IsContraAccount
                              select a;
            var equities = from a in _accountRepo.Table
                           where a.AccountClassId == 3 && a.ParentAccountId != null && !a.IsContraAccount
                           select a;

            var balanceSheet = new HashSet<BalanceSheet>();
            foreach (var asset in assets)
            {
                balanceSheet.Add(new BalanceSheet()
                {
                    AccountId = asset.Id,
                    AccountClassId = asset.AccountClassId,
                    AccountCode = asset.AccountCode,
                    AccountName = asset.AccountName,
                    Amount = asset.Balance - ((asset.IsContraAccount && asset.ContraAccounts.Count > 0) == true ? asset.ContraAccounts.FirstOrDefault().RelatedContraAccount.Balance : 0)
                });
            }
            foreach (var liability in liabilities)
            {
                balanceSheet.Add(new BalanceSheet()
                {
                    AccountId = liability.Id,
                    AccountClassId = liability.AccountClassId,
                    AccountCode = liability.AccountCode,
                    AccountName = liability.AccountName,
                    Amount = liability.Balance - ((liability.IsContraAccount && liability.ContraAccounts.Count > 0) == true ? liability.ContraAccounts.FirstOrDefault().RelatedContraAccount.Balance : 0)
                });
            }
            foreach (var equity in equities)
            {
                balanceSheet.Add(new BalanceSheet()
                {
                    AccountId = equity.Id,
                    AccountClassId = equity.AccountClassId,
                    AccountCode = equity.AccountCode,
                    AccountName = equity.AccountName,
                    Amount = equity.Balance - ((equity.IsContraAccount && equity.ContraAccounts.Count > 0) == true ? equity.ContraAccounts.FirstOrDefault().RelatedContraAccount.Balance : 0)
                });
            }
            return balanceSheet;
        }

        public ICollection<IncomeStatement> IncomeStatement(DateTime? from = default(DateTime?), DateTime? to = default(DateTime?))
        {
            var revenues = from r in _accountRepo.Table
                           where r.AccountClassId == 4 && r.ParentAccountId != null && !r.IsContraAccount
                           select r;

            var expenses = from e in _accountRepo.Table
                           where e.AccountClassId == 5 && e.ParentAccountId != null && !e.IsContraAccount
                           select e;

            var revenues_expenses = new HashSet<IncomeStatement>();
            foreach (var revenue in revenues)
            {
                revenues_expenses.Add(new IncomeStatement()
                {
                    AccountId = revenue.Id,
                    AccountCode = revenue.AccountCode,
                    AccountName = revenue.AccountName,
                    Amount = revenue.Balance - ((revenue.IsContraAccount && revenue.ContraAccounts.Count > 0) == true ? revenue.ContraAccounts.FirstOrDefault().RelatedContraAccount.Balance : 0),
                    IsExpense = false
                });
            }
            foreach (var expense in expenses)
            {
                revenues_expenses.Add(new IncomeStatement()
                {
                    AccountId = expense.Id,
                    AccountCode = expense.AccountCode,
                    AccountName = expense.AccountName,
                    Amount = expense.Balance - ((expense.IsContraAccount && expense.ContraAccounts.Count > 0) == true ? expense.ContraAccounts.FirstOrDefault().RelatedContraAccount.Balance : 0),
                    IsExpense = true
                });
            }
            return revenues_expenses;
        }

        public ICollection<MasterGeneralLedger> MasterGeneralLedger(DateTime? from = default(DateTime?), 
            DateTime? to = default(DateTime?), 
            string accountCode = null, 
            int? transactionNo = null)
        {
            var allDr = (from dr in _generalLedgerLineRepository.Table.AsEnumerable()
                         where dr.DrCr == DrOrCrSide.Dr
                         //&& GeneralLedgerHelper.IsBetween(dr.GLHeader.Date, (DateTime)fromDate, (DateTime)toDate) == true
                         select new MasterGeneralLedger
                         {
                             Id = dr.Id,
                             TransactionNo = dr.GeneralLedgerHeader.Id,
                             AccountId = dr.AccountId,
                             AccountCode = dr.Account.AccountCode,
                             AccountName = dr.Account.AccountName,
                             Date = dr.GeneralLedgerHeader.Date,
                             Debit = dr.Amount,
                             Credit = (decimal)0,
                             //CurrencyId = dr.CurrencyId,
                             DocumentType = Enum.GetName(typeof(DocumentTypes), dr.GeneralLedgerHeader.DocumentType)
                         });

            var allCr = (from cr in _generalLedgerLineRepository.Table.AsEnumerable()
                         where cr.DrCr == DrOrCrSide.Cr
                         //&& GeneralLedgerHelper.IsBetween(cr.GLHeader.Date, (DateTime)fromDate, (DateTime)toDate) == true
                         select new MasterGeneralLedger
                         {
                             Id = cr.Id,
                             TransactionNo = cr.GeneralLedgerHeader.Id,
                             AccountId = cr.AccountId,
                             AccountCode = cr.Account.AccountCode,
                             AccountName = cr.Account.AccountName,
                             Date = cr.GeneralLedgerHeader.Date,
                             Debit = (decimal)0,
                             Credit = cr.Amount,
                             //CurrencyId = cr.CurrencyId,
                             DocumentType = Enum.GetName(typeof(DocumentTypes), cr.GeneralLedgerHeader.DocumentType)
                         });

            var allDrcr = (from x in allDr
                           select new MasterGeneralLedger
                           {
                               Id = x.Id,
                               TransactionNo = x.TransactionNo,
                               AccountId = x.AccountId,
                               AccountCode = x.AccountCode,
                               AccountName = x.AccountName,
                               Date = x.Date,
                               Debit = x.Debit,
                               Credit = (decimal)0,
                               CurrencyId = x.CurrencyId,
                               DocumentType = x.DocumentType
                           }
                          ).Concat(from y in allCr
                                   select new MasterGeneralLedger
                                   {
                                       Id = y.Id,
                                       TransactionNo = y.TransactionNo,
                                       AccountId = y.AccountId,
                                       AccountCode = y.AccountCode,
                                       AccountName = y.AccountName,
                                       Date = y.Date,
                                       Debit = (decimal)0,
                                       Credit = y.Credit,
                                       CurrencyId = y.CurrencyId,
                                       DocumentType = y.DocumentType
                                   });

            if (!string.IsNullOrEmpty(accountCode))
                allDrcr = allDrcr.Where(a => a.AccountCode == accountCode);
            if(transactionNo != null)
                allDrcr = allDrcr.Where(a => a.TransactionNo == transactionNo);

            var sortedList = allDrcr.OrderBy(gl => gl.Id).ToList().Reverse<MasterGeneralLedger>();

            return sortedList.ToList();
        }

        public new IEnumerable<Bank> GetCashAndBanks()
        {
            var query = from b in _bankRepo.Table
                        select b;
            return query;
        }

        /// <summary>
        /// Input VAT is the value added tax added to the price when you purchase goods or services liable to VAT. If the buyer is registered in the VAT Register, the buyer can deduct the amount of VAT paid from his/her settlement with the tax authorities. 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, decimal>> ComputeInputTax(int vendorId, int itemId, decimal quantity, decimal amount, decimal discount)
        {
            decimal taxAmount = 0, amountXquantity = 0, discountAmount = 0, subTotalAmount = 0;

            var taxes = new List<KeyValuePair<int, decimal>>();
            var item = _inventoryService.GetItemById(itemId);

            amountXquantity = amount * quantity;

            if (discount > 0)
                discountAmount = (discount / 100) * amountXquantity;

            subTotalAmount = amountXquantity - discountAmount;

            var intersectionTaxes = _taxService.GetIntersectionTaxes(itemId, vendorId, BusinessCore.Domain.PartyTypes.Vendor);

            foreach (var tax in intersectionTaxes)
            {
                taxAmount = subTotalAmount - (subTotalAmount / (1 + (tax.Rate / 100)));
                taxes.Add(new KeyValuePair<int, decimal>(tax.Id, taxAmount));
            }

            return taxes;
        }

        /// <summary>
        /// Output VAT is the value added tax you calculate and charge on your own sales of goods and services
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [Obsolete]
        public List<KeyValuePair<int, decimal>> ComputeOutputTax(int customerId, int itemId, decimal quantity, decimal amount, decimal discount)
        {
            decimal taxAmount = 0, amountXquantity = 0, discountAmount = 0, subTotalAmount = 0;

            var item = _itemRepo.GetById(itemId);
            var customer = _customerRepo.GetById(customerId);
            var taxes = new List<KeyValuePair<int, decimal>>();

            amountXquantity = amount * quantity;

            if (discount > 0)
                discountAmount = (discount / 100) * amountXquantity;

            subTotalAmount = amountXquantity - discountAmount;

            var intersectionTaxes = _taxService.GetIntersectionTaxes(itemId, customerId, BusinessCore.Domain.PartyTypes.Customer);

            foreach (var tax in intersectionTaxes)
            {
                taxAmount = subTotalAmount - (subTotalAmount / (1 + (tax.Rate / 100)));
                taxes.Add(new KeyValuePair<int, decimal>(tax.Id, taxAmount));
            }

            return taxes;
        }

        public List<KeyValuePair<int, decimal>> ComputeOutputTax(int itemId, decimal quantity, decimal amount, decimal discount, IEnumerable<Tax> itemTaxes)
        
{
            decimal taxAmount = 0, amountXquantity = 0, discountAmount = 0, subTotalAmount = 0;

            var taxes = new List<KeyValuePair<int, decimal>>();

            amountXquantity = amount * quantity;

            if (discount > 0)
                discountAmount = (discount / 100) * amountXquantity;

            subTotalAmount = amountXquantity - discountAmount;

            //var intersectionTaxes = _taxService.GetIntersectionTaxes(itemId, customerId, BusinessCore.Domain.PartyTypes.Customer);

            foreach (var tax in itemTaxes)
            {
                taxAmount = subTotalAmount - (subTotalAmount / (1 + (tax.Rate / 100)));
                taxes.Add(new KeyValuePair<int, decimal>(tax.Id, taxAmount));
            }

            return taxes;
        }

        public new GeneralLedgerSetting GetGeneralLedgerSetting()
        {
            GeneralLedgerSetting glSetting = null;

            glSetting = _glSettingRepo.Table.FirstOrDefault();

            return glSetting;
        }

        public void UpdateGeneralLedgerSetting(GeneralLedgerSetting setting)
        {
            _glSettingRepo.Update(setting);
        }

        public void AddMainContraAccountSetting(int mainAccountId, int contraAccountId)
        {
            var contraAccount = _accountRepo.GetById(contraAccountId);
            var mainAccount = _accountRepo.GetById(mainAccountId);

            if (mainAccountId == contraAccountId)
                throw new Exception("Main account is same as contra account.");
            if (!contraAccount.IsContraAccount)
                throw new Exception("Account is not a contra account.");
            if(_maincontraAccount.Table.Any(a => a.MainAccountId == mainAccountId))
                throw new Exception("Main account already has contra account set.");

            _maincontraAccount.Insert(new MainContraAccount() { MainAccountId = mainAccountId, RelatedContraAccountId = contraAccountId });
        }

        public void UpdateAccount(Account account)
        {
            if(account.IsContraAccount && account.ContraAccounts.Count > 0)
                throw new Exception("An account cannot have contra account if the account is already contra account.");

            if (GetAccounts().Any(a => a.AccountCode == account.AccountCode && a.Id != account.Id))
                throw new Exception("Account code already exist.");

            if (account.ParentAccountId.HasValue && account.ParentAccountId.Value != -1)
            {
                var parent = GetAccount(account.ParentAccountId.Value);
                if(account.Id == parent.ParentAccountId)
                    throw new Exception("Cyclic parent/child account.");
            }

            _accountRepo.Update(account);
        }

        public void AddAccount(Account account)
        {
            if (account.IsContraAccount && account.ContraAccounts.Count > 0)
                throw new Exception("An account cannot have contra account if the account is already contra account.");

            if (GetAccounts().Any(a => a.AccountCode == account.AccountCode && a.Id != account.Id))
                throw new Exception("Account code already exist.");

            if (account.ParentAccountId.HasValue)
            {
                var parent = GetAccount(account.ParentAccountId.Value);
                if (account.Id == parent.ParentAccountId)
                    throw new Exception("Cyclic parent/child account.");
            }

            _accountRepo.Insert(account);
        }

        public JournalEntryHeader GetJournalEntry(int id, bool fromGL = false)
        {
            if(fromGL)
                return _journalEntryRepo.Table.Include(je => je.JournalEntryLines).Where(je => je.GeneralLedgerHeaderId == id).FirstOrDefault();
            return     _journalEntryRepo.Table.Include(je => je.JournalEntryLines).Where(je => je.Id == id).FirstOrDefault();
        }

        public void UpdateJournalEntry(JournalEntryHeader journalEntry, bool posted = false)
        {
            if (posted)
            {
                journalEntry.Posted = posted;

                if (journalEntry.GeneralLedgerHeaderId == 0)
                {
                    var glEntry = new GeneralLedgerHeader()
                    {
                        Date = DateTime.Now,
                        DocumentType = BusinessCore.Domain.DocumentTypes.JournalEntry,
                        Description = journalEntry.Memo,
                    };

                    foreach (var je in journalEntry.JournalEntryLines)
                    {
                        glEntry.GeneralLedgerLines.Add(new GeneralLedgerLine()
                        {
                            Account = GetAccounts().Where(a => a.Id == je.AccountId).FirstOrDefault(),
                            AccountId = je.AccountId,
                            DrCr = je.DrCr,
                            Amount = je.Amount,
                        });
                    }

                    if (ValidateGeneralLedgerEntry(glEntry))
                    {
                        journalEntry.GeneralLedgerHeader = glEntry;
                    }
                }
            }

            _journalEntryRepo.Update(journalEntry);

            //var glEntry = _generalLedgerRepository.Table.Where(gl => gl.Id == journalEntry.GeneralLedgerHeaderId).FirstOrDefault();

            //glEntry.Date = journalEntry.Date;

            //foreach (var je in journalEntry.JournalEntryLines)
            //{
            //    if (glEntry.GeneralLedgerLines.Any(l => l.AccountId == je.AccountId))
            //    {
            //        var existingLine = glEntry.GeneralLedgerLines.Where(l => l.AccountId == je.AccountId).FirstOrDefault();
            //        existingLine.Amount = je.Amount;
            //        existingLine.DrCr = je.DrCr;
            //    }
            //    else
            //    {
            //        glEntry.GeneralLedgerLines.Add(new GeneralLedgerLine()
            //        {
            //            AccountId = je.AccountId,
            //            DrCr = je.DrCr,
            //            Amount = je.Amount,
            //        });
            //    }
            //}

            //if (ValidateGeneralLedgerEntry(glEntry) && glEntry.ValidateAccountingEquation())
            //{
            //    journalEntry.GeneralLedgerHeader = glEntry;
            //    _journalEntryRepo.Update(journalEntry);
            //}
        }

        public GeneralLedgerHeader GetGeneralLedgerHeader(int id)
        {
            return _generalLedgerRepository.Table.Where(gl => gl.Id == id).FirstOrDefault();
        }

        public JournalEntryHeader CloseAccountingPeriod()
        {
            /*
            Example:

            The following example shows the closing entries based on the adjusted trial balance of Company A.

            Note	
            
            Date	        Account	                Debit	        Credit
            1	Jan 31	    Service Revenue	        85,600	
                            Income Summary		                    85,600
            2	Jan 31	    Income Summary	        77,364	
                            Wages Expense		                    38,200
                            Supplies Expense	                    18,480
                            Rent Expense		                    12,000
                            Miscellaneous Expense	                3,470
                            Electricity Expense		                2,470
                            Telephone Expense		                1,494
                            Depreciation Expense	                1,100
                            Interest Expense		                150
            3	Jan 31	    Income Summary	        8,236	
                            Retained Earnings		                8,236
            4	Jan 31	    Retained Earnings	    5,000	
                            Dividend		                        5,000
            
            Notes

            1. Service revenue account is debited and its balance it credited to income summary account. If a business has other income accounts, for example gain on sale account, then the debit side of the first closing entry will also include the gain on sale account and the income summary account will be credited for the sum of all income accounts.
            2. Each expense account is credited and the income summary is debited for the sum of the balances of expense accounts. This will reduce the balance in income summary account.
            3. Income summary account is debited and retained earnings account is credited for the an amount equal to the excess of service revenue over total expenses i.e. the net balance in income summary account after posting the first two closing entries. In this case $85,600 − $77,364 = $8,236. Please note that, if the balance in income summary account is negative at this stage, this closing entry will be opposite i.e. debit to retained earnings and credit to income summary.
            4. The last closing entry transfers the dividend or withdrawal account balance to the retained earnings account. Since dividend and withdrawal accounts are contra to the retained earnings account, they reduce the balance in the retained earnings.
            */

            var glSetting = _generalLedgerSettingRepo.Table.FirstOrDefault();

            var journalEntry = new JournalEntryHeader();
            journalEntry.Memo = "Closing entries";
            journalEntry.Date = DateTime.Now;
            journalEntry.Posted = false;
            journalEntry.VoucherType = JournalVoucherTypes.ClosingEntries;

            return journalEntry;
        }
    }
}
