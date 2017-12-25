//-----------------------------------------------------------------------
// <copyright file="SalesOrderLine.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using BusinessCore.Domain.Items;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Sales
{
    [Table("SalesOrderLine")]
    public partial class SalesOrderLine : BaseEntity, ICompanyBaseEntity
    {
        public int SalesOrderHeaderId { get; set; }
        public int ItemId { get; set; }
        public int MeasurementId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }
        public virtual SalesOrderHeader SalesOrderHeader { get; set; }
        public virtual Item Item { get; set; }
        public virtual Measurement Measurement { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
