using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Items
{
    public partial class ItemModel : BaseEntity, ICompanyBaseEntity
    {
        public ItemModel()
        {
            Items = new HashSet<Item>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        //public string Description { get; set; }//des

        public virtual ICollection<Item> Items { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
