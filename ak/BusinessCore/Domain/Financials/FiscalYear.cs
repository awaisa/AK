//-----------------------------------------------------------------------
// <copyright file="FiscalYear.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Financials
{
    [Table("FinancialYear")]
    public partial class FinancialYear : BaseEntity, ICompanyBaseEntity
    {
        [Required]
        [StringLength(10)]
        public string FiscalYearCode { get; set; }

        [Required]
        [StringLength(100)]
        public string FiscalYearName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

    }
}
