using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Domain.Financials
{
    //[Table("MainContraAccount")]
    public class MainContraAccount : BaseEntity
    {
        public int? MainAccountId { get; set; }
        public int? RelatedContraAccountId { get; set; }

        //[ForeignKey("MainAccountId")]
        //[InverseProperty("ContraAccounts")]
        public virtual Account MainAccount { get; set; }
        //[ForeignKey("RelatedContraAccountId")]        
        public virtual Account RelatedContraAccount { get; set; }
    }
}
