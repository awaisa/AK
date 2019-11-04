using AutoMapper;
using BusinessCore.Domain;
using BusinessCore.Domain.Financials;
using BusinessCore.Domain.Items;
using BusinessCore.Domain.Purchases;
using BusinessCore.Domain.Sales;
using BusinessCore.Domain.TaxSystem;
using System.Linq;
using WebApiCore.Models.Common;
using WebApiCore.Models.Customer;
using WebApiCore.Models.Financial;
using WebApiCore.Models.Inventory;
using WebApiCore.Models.Purchase;
using WebApiCore.Models.Vendor;

namespace WebApiCore.Models.Mappings
{
    public class FinancialMappingProfile : Profile
    {
        public FinancialMappingProfile()
        {
            CreateMap<Account, AccountModel>()
                .ForMember(x => x.Name, opt => opt.MapFrom(mf => mf.AccountName))
                .ForMember(x => x.Code, opt => opt.MapFrom(mf => mf.AccountCode));
            CreateMap<AccountModel, Account>()
                .ForPath(x => x.AccountName, opt => opt.MapFrom(mf => mf.Name))
                .ForPath(x => x.AccountCode, opt => opt.MapFrom(mf => mf.Code));

            CreateMap<Account, Financial.SearchRowModel>()
                .ForPath(x => x.AccountClass, opt => opt.MapFrom(mf => mf.AccountClass.Name));

            CreateMap<JournalEntryHeader, FinancialJournal.SearchRowModel>();
            CreateMap<JournalEntryHeader, FinancialJournal.JournalModel>();
            CreateMap<FinancialJournal.JournalModel, JournalEntryHeader>();

            CreateMap<FinancialJournal.JournalLineModel, JournalEntryLine>();
            CreateMap<JournalEntryLine, FinancialJournal.JournalLineModel>();
        }
    }
}
