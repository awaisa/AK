using BusinessCore.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore.Models.Inventory
{
    public class ItemModel
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
    }
}
