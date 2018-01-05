using BusinessCore.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore.Models.Sale
{
    public class SearchRowModel : BaseModel
    {
        public string No { get; set; }
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal? Total { get; set; }
    }
}
