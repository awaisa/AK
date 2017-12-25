using BusinessCore.Domain.Purchases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Purchases
{
    public partial class VendorMap : BaseEntityMap<Vendor>
    {
        public override void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("Vendor");

            TableColumns = () =>
            {
                builder.Property(p => p.No).HasColumnName("No");
                builder.Property(p => p.PartyId).HasColumnName("PartyId");
                builder.HasOne(t => t.Party).WithMany().HasForeignKey(t => t.PartyId);

                builder.Property(p => p.PartyId).HasColumnName("PartyId");
                builder.HasOne(t => t.Party).WithMany().HasForeignKey(t => t.PartyId);

                builder.Property(p => p.AccountsPayableAccountId).HasColumnName("AccountsPayableAccountId");
                builder.HasOne(t => t.AccountsPayableAccount).WithMany().HasForeignKey(t => t.AccountsPayableAccountId);

                builder.Property(p => p.PurchaseAccountId).HasColumnName("PurchaseAccountId");
                builder.HasOne(t => t.PurchaseAccount).WithMany().HasForeignKey(t => t.PurchaseAccountId);

                builder.Property(p => p.PurchaseDiscountAccountId).HasColumnName("PurchaseDiscountAccountId");
                builder.HasOne(t => t.PurchaseDiscountAccount).WithMany().HasForeignKey(t => t.PurchaseDiscountAccountId);

                builder.Property(p => p.PaymentTermId).HasColumnName("PaymentTermId");
                builder.HasOne(t => t.PaymentTerm).WithMany().HasForeignKey(t => t.PaymentTermId);

                builder.Property(p => p.TaxGroupId).HasColumnName("TaxGroupId");
                builder.HasOne(t => t.TaxGroup).WithMany().HasForeignKey(t => t.TaxGroupId);

                builder.Property(p => p.PartyId).HasColumnName("PartyId");
                builder.HasOne(t => t.Party).WithMany().HasForeignKey(t => t.PartyId);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
        
    }
}
