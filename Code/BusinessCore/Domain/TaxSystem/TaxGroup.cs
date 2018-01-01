using System.Collections.Generic;

namespace BusinessCore.Domain.TaxSystem
{
    public partial class TaxGroup : BaseEntity, ICompanyBaseEntity
    {
        public TaxGroup()
        {
            TaxGroupTax = new HashSet<TaxGroupTax>();
        }
        public string Description { get; set; }
        public bool TaxAppliedToShipping { get; set; }
        //public bool IsActive { get; set; }
        public virtual ICollection<TaxGroupTax> TaxGroupTax { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }

    public partial class TaxGroupTax :  BaseEntity, ICompanyBaseEntity
    {
        public int TaxId { get; set; }
        public int TaxGroupId { get; set; }
        public virtual Tax Tax { get; set; }
        public virtual TaxGroup TaxGroup { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
