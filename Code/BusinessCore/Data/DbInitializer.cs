using BusinessCore.Domain;
using BusinessCore.Domain.Auditing;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.Sales;
using BusinessCore.Domain.Security;
using BusinessCore.Domain.TaxSystem;
using BusinessCore.Security;
using BusinessCore.Utilities;
using GenFu;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace BusinessCore.Data
{
    public class DbInitializer
    {
        ApplicationContext _context;
        List<Account> _coa;
        List<TaxGroup> _taxGroups;
        string _cOAJSONfilename = Path.Combine(Directory.GetCurrentDirectory(), "csv.json");
        IAppPrincipal _appPrincipal;

        public DbInitializer(ApplicationContext context)
        {
            _context = context;

            _appPrincipal = _context.AppPrincipal;
        }

        public void DoIt()
        {
            if (_context.AuditableEntities.Count() == 0) InitEntityToAudit();

            if (_context.AccountClasses.Count() == 0) InitAccountClasses();

            if (_context.SecurityGroups.Count() == 0) InitSecurityGroupAndSecurityRole();

            var companies = InitCompanies();
            foreach (var company in companies)
            {
                //MockAppPrincipal(0, company.Id, "Joe", "Blog", "JoeBlog");
                _context.AppPrincipal = TestPrincipal.MockAppPrincipal(_appPrincipal.UserId, company.Id, _appPrincipal.Firstname, _appPrincipal.Surname, _appPrincipal.Username);

                InitUsers();

                FinancialYear fy = InitFiscalYear();

                List<PaymentTerm> paymentTerms = InitPaymentTerms(company.Id);

                _coa = InitChartOfAccounts();

                GeneralLedgerSetting glSetting = InitGeneralLedgerSetting();

                _taxGroups = InitTaxGroup();

                List<Bank> banks = InitBanks();

                InitVendors();

                var customers = InitCustomer();

                List<Item> items = InitItems();
            }

            _context.AppPrincipal = _appPrincipal;
        }

        void InitSecurityGroupAndSecurityRole()
        {
            var securityGroups = new List<SecurityGroup>();

            securityGroups.Add(new SecurityGroup() { GroupName = "Sales" });
            securityGroups.Add(new SecurityGroup() { GroupName = "Purchasing" });
            securityGroups.Add(new SecurityGroup() { GroupName = "Items" });
            securityGroups.Add(new SecurityGroup() { GroupName = "Financials" });
            securityGroups.Add(new SecurityGroup() { GroupName = "Administration" });

            var securityRoles = new List<SecurityRole>();

            securityRoles.Add(new SecurityRole() { RoleName = "Administrators" });
            securityRoles.Add(new SecurityRole() { RoleName = "Users" });

            _context.SecurityGroups.AddRange(securityGroups);

            _context.SecurityRoles.AddRange(securityRoles);

            _context.SaveChanges();
        }
        void InitUsers()
        {
            //var confGenFu.GenFuConfigurator< User>
            var users = A.ListOf<User>(2);
            users.ForEach(u => u.Id = 0);

            if (_context.AppPrincipal.CompanyId == 1)
                users.Add(new User() { Lastname = "System", Firstname = "Administrator", Username = "admin", Password="admin" });

            _context.Users.AddRange(users);

            _context.SaveChanges();
        }
        List<Company> InitCompanies()
        {
            var companies = A.ListOf<Company>(5);
            companies.ForEach(c => c.Id = 0);
            _context.Companies.AddRange(companies);
            _context.SaveChanges();
            return companies;
        }
        FinancialYear InitFiscalYear()
        {
            var financialYear = new FinancialYear() { FiscalYearCode = "FY1516", FiscalYearName = "FY 2016/2017", StartDate = new DateTime(2016, 01, 01), EndDate = new DateTime(2016, 12, 31), IsActive = true };
            _context.FiscalYears.Add(financialYear);
            _context.SaveChanges();

            return financialYear;
        }
        List<PaymentTerm> InitPaymentTerms(int companyId)
        {
            var paymentTerms = new List<PaymentTerm>()
            {
                new PaymentTerm()
                {
                    Description = "Payment due within 10 days",
                    PaymentType = PaymentTypes.AfterNoOfDays,
                    DueAfterDays = 10,
                    IsActive = true,
                    CompanyId = companyId
                },
                new PaymentTerm()
                {
                    Description = "Due 15th Of the Following Month 	",
                    PaymentType = PaymentTypes.DayInTheFollowingMonth,
                    DueAfterDays = 15,
                    IsActive = true,
                    CompanyId = companyId
                },
                new PaymentTerm()
                {
                    Description = "Cash Only",
                    PaymentType = PaymentTypes.Cash,
                    IsActive = true,
                    CompanyId = companyId
                }
            };
            _context.PaymentTerms.AddRange(paymentTerms);

            _context.SaveChanges();

            return paymentTerms;
        }
        List<AccountClass> InitAccountClasses()
        {
            var accountClasses = new List<AccountClass>()
            {
                new AccountClass() { Name = "Assets", NormalBalance = "Dr" },
                new AccountClass() { Name = "Liabilities", NormalBalance = "Cr" },
                new AccountClass() { Name = "Equity", NormalBalance = "Cr" },
                new AccountClass() { Name = "Revenue", NormalBalance = "Cr" },
                new AccountClass() { Name = "Expense", NormalBalance = "Dr" },
                new AccountClass() { Name = "Temporary", NormalBalance = "NA" },
            };
            _context.AccountClasses.AddRange(accountClasses);

            _context.SaveChanges();

            return accountClasses;
        }
        List<Account> InitChartOfAccounts()
        {
            var json = File.ReadAllText(_cOAJSONfilename);
            var importedAccounts = JsonConvert.DeserializeObject<JsonAccount[]>(json);
            List<Account> accounts = new List<Account>();
            foreach (var importedAccount in importedAccounts)
            {
                Account account = new Account();
                account.AccountCode = importedAccount.AccountCode.ToString();
                account.AccountName = importedAccount.AccountName;
                account.AccountClassId = importedAccount.AccountClassId;
                account.IsCash = importedAccount.IsCash;
                account.IsContraAccount = importedAccount.IsContraAccount;

                if (importedAccount.Sign == "DR")
                    account.DrOrCrSide = DrOrCrSide.Dr;
                else if (importedAccount.Sign == "CR")
                    account.DrOrCrSide = DrOrCrSide.Cr;
                else
                    account.DrOrCrSide = DrOrCrSide.NA;
                accounts.Add(account);
            }
            foreach (var account in accounts)
            {
                int code = int.Parse(account.AccountCode);
                var importedAccount = importedAccounts.FirstOrDefault(i => i.AccountCode == code);
                if (!string.IsNullOrEmpty(importedAccount.ParentAccountCode))
                {
                    var parentAccount = accounts.FirstOrDefault(a => a.AccountCode == importedAccount.ParentAccountCode);
                    if (parentAccount != null)
                    {
                        account.ParentAccount = parentAccount;
                    }
                }
            }
            _context.Accounts.AddRange(accounts);

            return accounts;
        }
        GeneralLedgerSetting InitGeneralLedgerSetting()
        {
            var glSetting = new GeneralLedgerSetting()
            {
                GoodsReceiptNoteClearingAccount = _coa.Where(a => a.AccountCode == AccountCodes.GoodsReceiptNoteClearing_10810).FirstOrDefault(),
                ShippingChargeAccount = _coa.Where(a => a.AccountCode == AccountCodes.ShippingCharge_40500).FirstOrDefault(),
                SalesDiscountAccount = _coa.Where(a => a.AccountCode == AccountCodes.SalesDiscount_40400).FirstOrDefault(),
            };
            _context.GeneralLedgerSettings.Add(glSetting);
            _context.SaveChanges();

            return glSetting;
        }
        List<TaxGroup> InitTaxGroup()
        {
            List<Tax> taxes = InitTax();

            var taxGroups = new List<TaxGroup>()
            {
                new TaxGroup()
                {
                    Description = "VAT",
                    TaxAppliedToShipping = false,
                    IsActive = true,
                },
                new TaxGroup()
                {
                    Description = "Export",
                    TaxAppliedToShipping = false,
                    IsActive = true,
                }
            };

            _context.TaxGroups.AddRange(taxGroups);

            var taxGroupTaxes = new List<TaxGroupTax>()
            {
                new TaxGroupTax()
                {
                    TaxGroup = taxGroups.FirstOrDefault(g=> g.Description == "VAT"),
                    Tax = taxes.FirstOrDefault(t=> t.TaxCode == "VAT5%")
                },
                new TaxGroupTax()
                {
                    TaxGroup = taxGroups.FirstOrDefault(g=> g.Description == "VAT"),
                    Tax = taxes.FirstOrDefault(t=> t.TaxCode == "VAT12%")
                },
                new TaxGroupTax()
                {
                    TaxGroup = taxGroups.FirstOrDefault(g=> g.Description == "Export"),
                    Tax = taxes.FirstOrDefault(t=> t.TaxCode == "exportTax1%")
                }
            };

            _context.TaxGroupTax.AddRange(taxGroupTaxes);

            var itemTaxGroups = new List<ItemTaxGroup>()
            {
                new ItemTaxGroup()
                {
                    Name = "Regular",
                    IsFullyExempt = false,
                },
                new ItemTaxGroup()
                {
                    Name = "Preferenced",
                    IsFullyExempt = false,
                }
            };

            _context.ItemTaxGroups.AddRange(itemTaxGroups);

            var ItemTaxGroupTaxes = new List<ItemTaxGroupTax>()
            {
                new ItemTaxGroupTax()
                {
                    ItemTaxGroup = itemTaxGroups.FirstOrDefault(g=> g.Name == "Regular"),
                    Tax = taxes.FirstOrDefault(t=> t.TaxCode == "VAT5%")
                },
                new ItemTaxGroupTax()
                {
                    ItemTaxGroup = itemTaxGroups.FirstOrDefault(g=> g.Name == "Preferenced"),
                    Tax = taxes.FirstOrDefault(t=> t.TaxCode == "VAT12%")
                }
            };

            _context.ItemTaxGroupTax.AddRange(ItemTaxGroupTaxes);

            _context.SaveChanges();

            return taxGroups;
        }
        List<Tax> InitTax()
        {
            // NOTE: each tax should have its own tax account.
            var salesTaxAccount = _coa.Where(a => a.AccountCode == AccountCodes.SalesTax_20300).FirstOrDefault();
            var purchaseTaxAccount = _coa.Where(a => a.AccountCode == AccountCodes.PurchaseTax_50700).FirstOrDefault();

            List<Tax> taxes = new List<Tax>()
            {
                new Tax()
                {
                    TaxCode = "VAT5%",
                    TaxName = "VAT 5%",
                    Rate = 5,
                    IsActive = true,
                    SalesAccountId = salesTaxAccount.Id,
                    PurchasingAccountId = purchaseTaxAccount.Id
                },
                new Tax()
                {
                    TaxCode = "VAT10%",
                    TaxName = "VAT 10%",
                    Rate = 10,
                    IsActive = true,
                    SalesAccountId = salesTaxAccount.Id,
                    PurchasingAccountId = purchaseTaxAccount.Id
                },
                new Tax()
                {
                    TaxCode = "VAT12%",
                    TaxName = "VAT 12%",
                    Rate = 12,
                    IsActive = true,
                    SalesAccountId = salesTaxAccount.Id,
                    PurchasingAccountId = purchaseTaxAccount.Id
                },
                new Tax()
                {
                    TaxCode = "exportTax1%",
                    TaxName = "Export Tax 1%",
                    Rate = 1,
                    IsActive = true,
                    SalesAccountId = salesTaxAccount.Id,
                    PurchasingAccountId = purchaseTaxAccount.Id
                }
            };

            _context.Taxes.AddRange(taxes);
            _context.SaveChanges();
            return taxes;
        }
        List<Vendor> InitVendors()
        {
            List<Vendor> vendors = A.ListOf<Vendor>(10);
            vendors.ForEach(vendor =>
            {
                vendor.Id = 0;
                vendor.AccountsPayableAccountId = _coa.Where(a => a.AccountCode == AccountCodes.AccountsPayable_20110).FirstOrDefault().Id;
                vendor.PurchaseAccountId = _coa.Where(a => a.AccountCode == AccountCodes.Purchase_50200).FirstOrDefault().Id;
                vendor.PurchaseDiscountAccountId = _coa.Where(a => a.AccountCode == AccountCodes.PurchaseDiscount_50400).FirstOrDefault().Id;
                vendor.Party = A.New<Party>();
                vendor.Party.Id = 0;
                vendor.Party.PartyType = BusinessCore.Domain.PartyTypes.Vendor;
                vendor.Party.IsActive = true;

                var contacts = A.ListOf<Contact>(3);
                contacts.ForEach(c =>
                {
                    c.Id = 0;
                    c.ContactType = ContactTypes.Vendor;
                    c.Party = vendor.Party;

                });
                vendor.Party.Contacts = contacts;
                contacts[0].IsPrimary = true;
            });

            _context.Vendors.AddRange(vendors);

            _context.SaveChanges();

            return vendors;


        }
        List<Customer> InitCustomer()
        {
            var accountAR = _coa.Where(e => e.AccountCode == AccountCodes.AccountsReceivable_10120).FirstOrDefault();
            var accountSales = _coa.Where(e => e.AccountCode == AccountCodes.Sales_40100).FirstOrDefault();
            var accountAdvances = _coa.Where(e => e.AccountCode == AccountCodes.CutomerAdvances_20120).FirstOrDefault();
            var accountSalesDiscount = _coa.Where(e => e.AccountCode == AccountCodes.SalesDiscount_40400).FirstOrDefault();

            List<Customer> customers = A.ListOf<Customer>(10);
            customers.ForEach(customer =>
            {
                customer.Id = 0;
                customer.AccountsReceivableAccountId = accountAR.Id;
                customer.SalesAccountId = accountSales.Id;
                customer.CustomerAdvancesAccountId = accountAdvances.Id;
                customer.SalesDiscountAccountId = accountSalesDiscount.Id;
                customer.TaxGroupId = _taxGroups.Where(tg => tg.Description == "VAT").FirstOrDefault().Id;

                customer.Party = A.New<Party>();
                customer.Party.Id = 0;
                customer.Party.PartyType = BusinessCore.Domain.PartyTypes.Vendor;
                customer.Party.IsActive = true;

                var contacts = A.ListOf<Contact>(3);
                contacts.ForEach(c =>
                {
                    c.Id = 0;
                    c.ContactType = ContactTypes.Customer;
                    c.Party = customer.Party;

                });
                customer.Party.Contacts = contacts;
                contacts[0].IsPrimary = true;
            });

            _context.Customers.AddRange(customers);
            _context.SaveChanges();

            return customers;
        }
        List<Item> InitItems()
        {
            _context.Measurements.Add(new Measurement() { Code = "EA", Description = "Each" });
            _context.Measurements.Add(new Measurement() { Code = "PK", Description = "Pack" });
            _context.Measurements.Add(new Measurement() { Code = "MO", Description = "Monthly" });
            _context.Measurements.Add(new Measurement() { Code = "HR", Description = "Hour" });
            _context.SaveChanges();

            // Accounts = Sales A/C (40100), Inventory (10800), COGS (50300), Inv Adjustment (50500), Item Assm Cost (10900)
            var sales = _coa.Where(a => a.AccountCode == AccountCodes.Sales_40100).FirstOrDefault();
            var inventory = _coa.Where(a => a.AccountCode == AccountCodes.Inventory_10800).FirstOrDefault();
            var invAdjusment = _coa.Where(a => a.AccountCode == AccountCodes.PurchasePriceVariance_50500).FirstOrDefault();
            var cogs = _coa.Where(a => a.AccountCode == AccountCodes.CostOfGoodsSold_50300).FirstOrDefault();
            var assemblyCost = _coa.Where(a => a.AccountCode == AccountCodes.AssemblyCost_10900).FirstOrDefault();

            var itemCat = new ItemCategory()
            {
                Name = "Charges",
                Measurement = _context.Measurements.Where(m => m.Code == "EA").FirstOrDefault(),
                ItemType = ItemTypes.Charge,
                SalesAccount = sales,
                InventoryAccount = inventory,
                AdjustmentAccount = invAdjusment,
                CostOfGoodsSoldAccount = cogs,
                AssemblyAccount = assemblyCost
            };
            itemCat.Items.Add(new Item()
            {
                Description = "HOA Dues",
                SellDescription = "HOA Dues",
                PurchaseDescription = "HOA Dues",
                Price = 350,
                SmallestMeasurement = _context.Measurements.Where(m => m.Code == "MO").FirstOrDefault(),
                SellMeasurement = _context.Measurements.Where(m => m.Code == "MO").FirstOrDefault(),
                SalesAccount = _coa.Where(a => a.AccountCode == AccountCodes.HomeOwners_40200).FirstOrDefault(),
                No = "1"
            });
            _context.ItemCategories.Add(itemCat);

            var componentCategory = new ItemCategory()
            {
                Name = "Components",
                Measurement = _context.Measurements.Where(m => m.Code == "EA").FirstOrDefault(),
                ItemType = ItemTypes.Purchased,
                SalesAccount = sales,
                InventoryAccount = inventory,
                AdjustmentAccount = invAdjusment,
                CostOfGoodsSoldAccount = cogs,
                AssemblyAccount = assemblyCost
            };

            var carStickerItem = new Item()
            {
                Description = "Car Sticker",
                SellDescription = "Car Sticker",
                PurchaseDescription = "Car Sticker",
                Price = 100,
                Cost = 40,
                SmallestMeasurement = _context.Measurements.Where(m => m.Code == "EA").FirstOrDefault(),
                SellMeasurement = _context.Measurements.Where(m => m.Code == "EA").FirstOrDefault(),
                PurchaseMeasurement = _context.Measurements.Where(m => m.Code == "EA").FirstOrDefault(),
                SalesAccount = sales,
                InventoryAccount = inventory,
                CostOfGoodsSoldAccount = cogs,
                InventoryAdjustmentAccount = invAdjusment,
                ItemTaxGroup = _context.ItemTaxGroups.Where(m => m.Name == "Regular").FirstOrDefault(),
                No = "2"
            };

            var otherItem = new Item()
            {
                Description = "Optical Mouse",
                SellDescription = "Optical Mouse",
                PurchaseDescription = "Optical Mouse",
                Price = 80,
                Cost = 30,
                SmallestMeasurement = _context.Measurements.Where(m => m.Code == "EA").FirstOrDefault(),
                SellMeasurement = _context.Measurements.Where(m => m.Code == "EA").FirstOrDefault(),
                PurchaseMeasurement = _context.Measurements.Where(m => m.Code == "EA").FirstOrDefault(),
                SalesAccount = sales,
                InventoryAccount = inventory,
                CostOfGoodsSoldAccount = cogs,
                InventoryAdjustmentAccount = invAdjusment,
                ItemTaxGroup = _context.ItemTaxGroups.Where(m => m.Name == "Regular").FirstOrDefault(),
                No = "3"
            };

            componentCategory.Items.Add(carStickerItem);
            componentCategory.Items.Add(otherItem);
            _context.ItemCategories.Add(componentCategory);

            _context.ItemCategories.Add(new ItemCategory()
            {
                Name = "Services",
                Measurement = _context.Measurements.Where(m => m.Code == "HR").FirstOrDefault(),
                ItemType = ItemTypes.Service,
                SalesAccount = sales,
                InventoryAccount = inventory,
                AdjustmentAccount = invAdjusment,
                CostOfGoodsSoldAccount = cogs,
                AssemblyAccount = assemblyCost
            });

            _context.ItemCategories.Add(new ItemCategory()
            {
                Name = "Systems",
                Measurement = _context.Measurements.Where(m => m.Code == "EA").FirstOrDefault(),
                ItemType = ItemTypes.Manufactured,
                SalesAccount = sales,
                InventoryAccount = inventory,
                AdjustmentAccount = invAdjusment,
                CostOfGoodsSoldAccount = cogs,
                AssemblyAccount = assemblyCost
            });

            _context.SaveChanges();

            return _context.Items.ToList();
        }
        List<Bank> InitBanks()
        {
            var bank = new Bank()
            {
                AccountId = _coa.Where(a => a.AccountCode == AccountCodes.RegularCheckingAccount_10111).FirstOrDefault().Id,
                Name = "General Fund",
                Type = BankTypes.CheckingAccount,
                BankName = "GFB",
                Number = "1234567890",
                Address = "123 Main St.",
                IsDefault = true,
                IsActive = true
            };
            _context.Banks.Add(bank);

            bank = new Bank()
            {
                AccountId = _coa.Where(a => a.AccountCode == AccountCodes.CashInHand_10113).FirstOrDefault().Id,
                Name = "Petty Cash Account",
                Type = BankTypes.CashAccount,
                IsDefault = false,
                IsActive = true
            };
            _context.Banks.Add(bank);
            _context.SaveChanges();

            return _context.Banks.ToList();
        }
        void InitEntityToAudit()
        {
            var auditAccount = new AuditableEntity();
            auditAccount.EntityName = "Account";
            auditAccount.EnableAudit = true;

            auditAccount.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "CompanyId", EnableAudit = true });
            auditAccount.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "AccountClassId", EnableAudit = true });
            auditAccount.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "ParentAccountId", EnableAudit = true });
            auditAccount.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "DrOrCrSide", EnableAudit = true });
            auditAccount.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "AccountCode", EnableAudit = true });
            auditAccount.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "AccountName", EnableAudit = true });
            auditAccount.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "Description", EnableAudit = true });
            auditAccount.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "IsCash", EnableAudit = true });
            auditAccount.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "IsContraAccount", EnableAudit = true });

            _context.AuditableEntities.Add(auditAccount);

            var auditJE = new AuditableEntity();
            auditJE.EntityName = "JournalEntryHeader";
            auditJE.EnableAudit = true;

            auditJE.AuditableAttributes.Add(new AuditableAttribute() { AttributeName = "Posted", EnableAudit = true });

            _context.AuditableEntities.Add(auditJE);

            _context.SaveChanges();
        }
    }
}
