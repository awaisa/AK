using BusinessCore.Domain.Financials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Items
{
    public partial class ItemBrand : BaseEntity, ICompanyBaseEntity
    {
        public ItemBrand()
        {
            Items = new HashSet<Item>();
        }

        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
