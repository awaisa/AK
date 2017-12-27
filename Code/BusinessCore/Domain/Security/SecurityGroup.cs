﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Security
{
    [Table("SecurityGroup", Schema = "dbo")]
    public class SecurityGroup : BaseEntity
    {
        //[Key]
        //public int SecurityGroupId { get; set; }
        public string GroupName { get; set; }

        public virtual ICollection<SecurityPermission> Permissions { get; set; }

        public SecurityGroup()
        {
            Permissions = new HashSet<SecurityPermission>();
        }
    }
}
