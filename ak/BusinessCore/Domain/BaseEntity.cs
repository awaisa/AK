//-----------------------------------------------------------------------
// <copyright file="BaseEntity.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using BusinessCore.Domain.Security;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain
{
    public abstract partial class BaseEntity
    {
        //[System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [System.ComponentModel.DataAnnotations.Key]
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public virtual int Id { get; set; }

        public virtual System.DateTime? CreatedOn { get; set; }
        public virtual int? CreatedById { get; set; }
        public virtual System.DateTime? ModifiedOn { get; set; }
        public virtual int? ModifiedById { get; set; }

        //[ForeignKey("CreatedById")]
        //public virtual User CreatedBy { get; set; }

        //[ForeignKey("ModifiedById")]
        //public virtual User ModifiedBy { get; set; }

        public virtual bool IsInactive { get; set; }
    }

    public interface ICompanyBaseEntity
    {
        int CompanyId { get; set; }
    }
}
