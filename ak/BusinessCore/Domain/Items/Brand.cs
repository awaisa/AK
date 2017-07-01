using BusinessCore.Domain.Financials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Items
{
    [Table("Brand")]
    public partial class Brand : BaseEntity, ICompanyBaseEntity
    {
        public Brand()
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
