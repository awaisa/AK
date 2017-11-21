using BusinessCore.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore.Models
{
    public class InventorySearchModel
    {
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        //public List<string> data { get; set; }
        public List<InventorySearchRecordModel> data { get; set; }
    }

    public class InventorySearchRecordModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

    public class ItemInModel
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string SellDescription { get; set; }
        [Required]
        public string PurchaseDescription { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public decimal? Cost { get; set; }

        public Item To()
        {
            var item = new Item();
            item.Id = Id;
            item.Code = Code;
            item.Description = Description;
            item.SellDescription = SellDescription;
            item.PurchaseDescription = PurchaseDescription;
            item.Price = Price;
            item.Cost = Cost;
            return item;
        }
    }
}
