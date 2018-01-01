using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain
{
    public partial class CompanySetting : BaseEntity
    {
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
