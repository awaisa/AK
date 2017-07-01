//-----------------------------------------------------------------------
// <copyright file="SequenceNumber.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain
{
    [Table("SequenceNumber")]
    public partial class SequenceNumber : BaseEntity, ICompanyBaseEntity
    {
        public SequenceNumberTypes SequenceNumberType { get; set; }
        public string Description { get; set; }
        public string Prefix { get; set; }
        public int NextNumber { get; set; }
        public bool UsePrefix { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
