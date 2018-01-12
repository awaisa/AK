using BusinessCore.Domain;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.TaxSystem;
using System;
using System.Collections.Generic;

namespace BusinessCore.Services.Financial
{
    public partial interface IFinancialService
    {
        bool ValidateGeneralLedgerEntry(GeneralLedgerHeader glEntry);
        void SaveGeneralLedgerEntry(GeneralLedgerHeader entry);
        GeneralLedgerLine CreateGeneralLedgerLine(DrOrCrSide DrCr, int accountId, decimal amount);
        GeneralLedgerHeader CreateGeneralLedgerHeader(DocumentTypes documentType, DateTime Date, string description);
        IEnumerable<Account> GetAccounts();
        IEnumerable<JournalEntryHeader> GetJournalEntries();
        void AddJournalEntry(JournalEntryHeader journalEntry);
        ICollection<TrialBalance> TrialBalance(DateTime? from = null, DateTime? to = null);
        ICollection<BalanceSheet> BalanceSheet(DateTime? from = null, DateTime? to = null);
        ICollection<IncomeStatement> IncomeStatement(DateTime? from = null, DateTime? to = null);
        ICollection<MasterGeneralLedger> MasterGeneralLedger(DateTime? from = null, DateTime? to = null, string accountCode = null, int? transactionNo = null);
        FinancialYear CurrentFiscalYear();
        IEnumerable<Tax> GetTaxes();
        IEnumerable<ItemTaxGroup> GetItemTaxGroups();
        IEnumerable<TaxGroup> GetTaxGroups();
        IEnumerable<Bank> GetCashAndBanks();
        List<KeyValuePair<int, decimal>> ComputeInputTax(int vendorId, int itemId, decimal quantity, decimal amount, decimal discount);
        List<KeyValuePair<int, decimal>> ComputeOutputTax(int customerId, int itemId, decimal quantity, decimal amount, decimal discount);
        List<KeyValuePair<int, decimal>> ComputeOutputTax(int itemId, decimal quantity, decimal amount, decimal discount, IEnumerable<BusinessCore.Domain.TaxSystem.Tax> itemTaxes);
        GeneralLedgerSetting GetGeneralLedgerSetting();
        void UpdateGeneralLedgerSetting(GeneralLedgerSetting setting);
        void AddMainContraAccountSetting(int masterAccountId, int contraAccountId);
        void UpdateAccount(Account account);
        JournalEntryHeader GetJournalEntry(int id, bool fromGL = false);
        void UpdateJournalEntry(JournalEntryHeader journalEntry, bool posted = false);
        GeneralLedgerHeader GetGeneralLedgerHeader(int id);
        Account GetAccountByAccountCode(string accountcode);
        Account GetAccount(int id);
        void AddAccount(Account account);
        JournalEntryHeader CloseAccountingPeriod();
    }
}
