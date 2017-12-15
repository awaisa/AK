using BusinessCore.Domain.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore.Models.Purchase
{
    public class InvoiceRowModel
    {
        public int Id { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public decimal? Total { get; set; }
    }
}
