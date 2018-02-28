using BusinessCore.Domain;
using WebApiCore.Models.Common;

namespace WebApiCore.Models.Financial
{
    public class FinancialAccountModel : BaseModel
    {
        public int AccountClassId { get; set; }
        public int? ParentAccountId { get; set; }
        public DrOrCrSide DrOrCrSide { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string Description { get; set; }
        public bool IsCash { get; set; }
        public bool IsContraAccount { get; set; }
    }
}
