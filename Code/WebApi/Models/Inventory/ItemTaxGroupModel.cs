using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore.Models.Inventory
{
    public class ItemTaxGroupModel:BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
