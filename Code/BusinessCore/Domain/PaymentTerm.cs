using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain
{
    public partial class PaymentTerm : BaseEntity, ICompanyBaseEntity
    {
        public string Description { get; set; }
        public PaymentTypes PaymentType { get; set; }
        public int? DueAfterDays { get; set; }
        //public bool IsActive { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
