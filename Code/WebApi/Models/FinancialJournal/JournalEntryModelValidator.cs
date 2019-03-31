using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using WebApiCore.Models.Common;

namespace WebApiCore.Models.FinancialJournal
{
    public class JournalEntryModelValidator : AbstractValidator<JournalModel>
    {
        public JournalEntryModelValidator()
        {
            RuleFor(m => m.ReferenceNo).NotEmpty();
            RuleFor(m => m.Memo).NotEmpty();
            
            RuleFor(m => m.JournalEntryLines).NotEmpty();

            RuleForEach(m => m.JournalEntryLines).SetValidator(new JournalLineModelValidator());
        }
    }

    public class JournalLineModelValidator : AbstractValidator<JournalLineModel>
    {
        public JournalLineModelValidator()
        {
            RuleFor(m => m.AccountId).NotEmpty();
            RuleFor(m => m.Amount).NotEmpty();
            RuleFor(m => m.DrCr).NotEmpty();
        }
    }
}
