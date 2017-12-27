using System;
using System.Collections.Generic;

namespace WebApiCore.Models.Sale
{
    public class InvoiceModel : BaseModel
    {
        public string No { get; set; }
        public DateTime Date { get; set; }
        public int? VendorId { get; set; }
        public string VendorInvoiceNo { get; set; }
        public string Description { get; set; }
        public decimal? Total { get; set; }
        public List<InvoiceItemModel> InvoiceItems { get; set; }
    }

    public class InvoiceItemModel : BaseModel
    {
        public int ItemId { get; set; }
        public int? MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal? Cost { get; set; }
        public decimal? Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }
    }
}
