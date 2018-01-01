namespace BusinessCore.Domain.Financials
{
    public partial class GeneralLedgerSetting : BaseEntity, ICompanyBaseEntity
    {
        public GeneralLedgerSetting()
        {
        }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public int? PayableAccountId { get; set; }
        public int? PurchaseDiscountAccountId { get; set; }
        public int? GoodsReceiptNoteClearingAccountId { get; set; }
        public int? SalesDiscountAccountId { get; set; }
        public int? ShippingChargeAccountId { get; set; }
        public int? PermanentAccountId { get; set; }
        public int? IncomeSummaryAccountId { get; set; }

        public virtual Account PayableAccount { get; set; }
        public virtual Account PurchaseDiscountAccount { get; set; }
        public virtual Account GoodsReceiptNoteClearingAccount { get; set; }
        public virtual Account SalesDiscountAccount { get; set; }
        public virtual Account ShippingChargeAccount { get; set; }
        public virtual Account PermanentAccount { get; set; }
        public virtual Account IncomeSummaryAccount { get; set; }
    }
}
