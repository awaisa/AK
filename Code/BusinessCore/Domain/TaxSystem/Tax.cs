using BusinessCore.Domain.Financials;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.TaxSystem
{
    public partial class Tax : BaseEntity, ICompanyBaseEntity
    {
        public Tax()
        {
            TaxGroupTaxes = new HashSet<TaxGroupTax>();
            ItemTaxGroupTaxes = new HashSet<ItemTaxGroupTax>();
        }

        public int? SalesAccountId { get; set; }
        public int? PurchasingAccountId { get; set; }
        [Required]
        [StringLength(50)]
        public string TaxName { get; set; }
        [Required]
        [StringLength(16)]
        public string TaxCode { get; set; }
        public decimal Rate { get; set; }
        //public bool IsActive { get; set; }
        public virtual Account SalesAccount { get; set; }
        public virtual Account PurchasingAccount { get; set; }

        public virtual ICollection<TaxGroupTax> TaxGroupTaxes { get; set; }
        public virtual ICollection<ItemTaxGroupTax> ItemTaxGroupTaxes { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
