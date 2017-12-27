//-----------------------------------------------------------------------
// <copyright file="Contact.cs" company="AccountGo">
// Copyright (c) AccountGo. All rights reserved.
// <author>Marvin Perez</author>
// <date>1/11/2015 9:48:38 AM</date>
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain
{
    [Table("Contact")]
    public partial class Contact : BaseEntity, ICompanyBaseEntity
    {
        public Contact()
        {
        }
        /// <summary>
        /// Check ContactyType to determine whether CompanyNo is Customer No or Vendor No
        /// </summary>
        public ContactTypes ContactType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int PartyId { get; set; }
        public virtual Party Party { get; set; }
        public bool IsPrimary { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }
    }
}
