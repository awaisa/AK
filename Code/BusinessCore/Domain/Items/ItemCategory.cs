using BusinessCore.Domain.Financials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Items
{
    public partial class ItemCategory : BaseEntity, ICompanyBaseEntity
    {
        public ItemCategory()
        {
            Items = new HashSet<Item>();
        }        
        
        public ItemTypes ItemType { get; set; }
        public int? ItemCategoryId { get; set; }
        public int? MeasurementId { get; set; }
        public int? SalesAccountId { get; set; }
        public int? InventoryAccountId { get; set; }
        public int? CostOfGoodsSoldAccountId { get; set; }
        public int? AdjustmentAccountId { get; set; }
        public int? AssemblyAccountId { get; set; }
        public string Name { get; set; }
        public virtual Measurement Measurement { get; set; }
        public virtual Account SalesAccount { get; set; }
        public virtual Account InventoryAccount { get; set; }
        public virtual Account CostOfGoodsSoldAccount { get; set; }
        public virtual Account AdjustmentAccount { get; set; }
        public virtual Account AssemblyAccount { get; set; }
        //public int? ModelId { get; set; }
        //public int? BrandId { get; set; }
        public virtual ICollection<Item> Items { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
