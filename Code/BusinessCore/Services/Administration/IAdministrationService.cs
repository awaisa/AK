using System.Collections.Generic;
using BusinessCore.Domain;
using BusinessCore.Domain.TaxSystem;
using BusinessCore.Domain.Financials;

namespace BusinessCore.Services.Administration
{
    public interface IAdministrationService
    {
        ICollection<Tax> GetAllTaxes(bool includeInActive);
        ICollection<ItemTaxGroup> GetItemTaxGroups(bool includeInActive);
        ICollection<TaxGroup> GetTaxGroups(bool includeInActive);
        void AddNewTax(Tax tax);
        void UpdateTax(Tax tax);
        void DeleteTax(int id);
        void InitializeCompany();
        //Company GetCompany(int companyId);
        ICollection<PaymentTerm> GetPaymentTerms();
        ICollection<FinancialYear> GetFinancialYears();
        bool IsAccountsExists();
    }
}
