using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Items
{
    public partial class Measurement : BaseEntity, ICompanyBaseEntity
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
