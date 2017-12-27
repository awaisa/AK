using BusinessCore.Domain.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Financials
{
    public partial class GeneralLedgerSettingMap : BaseEntityMap<GeneralLedgerSetting>
    {
        public override void Configure(EntityTypeBuilder<GeneralLedgerSetting> builder)
        {
            builder.ToTable("GeneralLedgerSetting");

            TableColumns = () =>
            {
                builder.Property(p => p.PayableAccountId).HasColumnName("PayableAccountId");
                builder.HasOne(t => t.PayableAccount).WithMany().HasForeignKey(t => t.PayableAccountId);

                builder.Property(p => p.PurchaseDiscountAccountId).HasColumnName("PurchaseDiscountAccountId");
                builder.HasOne(t => t.PurchaseDiscountAccount).WithMany().HasForeignKey(t => t.PurchaseDiscountAccountId);

                builder.Property(p => p.GoodsReceiptNoteClearingAccountId).HasColumnName("GoodsReceiptNoteClearingAccountId");
                builder.HasOne(t => t.GoodsReceiptNoteClearingAccount).WithMany().HasForeignKey(t => t.GoodsReceiptNoteClearingAccountId);

                builder.Property(p => p.SalesDiscountAccountId).HasColumnName("SalesDiscountAccountId");
                builder.HasOne(t => t.SalesDiscountAccount).WithMany().HasForeignKey(t => t.SalesDiscountAccountId);

                builder.Property(p => p.ShippingChargeAccountId).HasColumnName("ShippingChargeAccountId");
                builder.HasOne(t => t.ShippingChargeAccount).WithMany().HasForeignKey(t => t.ShippingChargeAccountId);

                builder.Property(p => p.PermanentAccountId).HasColumnName("PermanentAccountId");
                builder.HasOne(t => t.PermanentAccount).WithMany().HasForeignKey(t => t.PermanentAccountId);

                builder.Property(p => p.IncomeSummaryAccountId).HasColumnName("IncomeSummaryAccountId");
                builder.HasOne(t => t.IncomeSummaryAccount).WithMany().HasForeignKey(t => t.IncomeSummaryAccountId);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }

    }
}
