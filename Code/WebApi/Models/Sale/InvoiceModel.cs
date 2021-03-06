﻿using BusinessCore.Domain;
using BusinessCore.Domain.Sales;
using System;
using System.Collections.Generic;
using WebApiCore.Models.Common;

namespace WebApiCore.Models.Sale
{
    public class InvoiceModel : BaseModel
    {
        public string No { get; set; }
        public DateTime Date { get; set; }
        public int? CustomerId { get; set; }
        public string Description { get; set; }
        public decimal? Total { get; set; } // bind ComputeTotalAmount
        public decimal ShippingHandlingCharge{ get; set; }
        public SalesInvoiceStatus Status { get; set; }
        public List<InvoiceItemModel> InvoiceItems { get; set; }
    }

    public class InvoiceItemModel : BaseModel
    {
        public int ItemId { get; set; }
        public int? MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Amount { get; set; }
        public List<TaxModel> Taxes { get; set; }
        public decimal? Discount { get; set; }
    }
}
