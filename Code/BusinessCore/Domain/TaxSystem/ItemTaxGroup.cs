using System.Collections.Generic;

namespace BusinessCore.Domain.TaxSystem
{
    public partial class ItemTaxGroup : BaseEntity, ICompanyBaseEntity
    {
        public ItemTaxGroup()
        {
            ItemTaxGroupTax = new HashSet<ItemTaxGroupTax>();
        }

        public string Name { get; set; }
        public bool IsFullyExempt { get; set; }

        public virtual ICollection<ItemTaxGroupTax> ItemTaxGroupTax { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }

    public partial class ItemTaxGroupTax : BaseEntity, ICompanyBaseEntity
    {
        public int TaxId { get; set; }
        public int ItemTaxGroupId { get; set; }
        public bool IsExempt { get; set; }
        
        public virtual Tax Tax { get; set; }
        public virtual ItemTaxGroup ItemTaxGroup { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
