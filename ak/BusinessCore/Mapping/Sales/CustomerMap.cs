using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Sales;

namespace BusinessCore.Mapping.Sales
{
    public partial class CustomerMap : BaseEntityMap<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer");

            TableColumns = () =>
            {
                builder.Property(p => p.No).HasColumnName("No");
                builder.Property(p => p.PartyId).HasColumnName("PartyId");
                builder.HasOne(t => t.Party).WithMany().HasForeignKey(t => t.PartyId);

                builder.Property(p => p.PrimaryContactId).HasColumnName("PrimaryContactId");
                builder.HasOne(t => t.PrimaryContact).WithMany().HasForeignKey(t => t.PrimaryContactId);

                builder.Property(p => p.TaxGroupId).HasColumnName("TaxGroupId");
                builder.HasOne(t => t.TaxGroup).WithMany().HasForeignKey(t => t.TaxGroupId);

                builder.Property(p => p.AccountsReceivableAccountId).HasColumnName("AccountsReceivableAccountId");
                builder.HasOne(t => t.AccountsReceivableAccount).WithMany().HasForeignKey(t => t.AccountsReceivableAccountId);

                builder.Property(p => p.SalesAccountId).HasColumnName("SalesAccountId");
                builder.HasOne(t => t.SalesAccount).WithMany().HasForeignKey(t => t.SalesAccountId);

                builder.Property(p => p.SalesDiscountAccountId).HasColumnName("SalesDiscountAccountId");
                builder.HasOne(t => t.SalesDiscountAccount).WithMany().HasForeignKey(t => t.SalesDiscountAccountId);

                builder.Property(p => p.PromptPaymentDiscountAccountId).HasColumnName("PromptPaymentDiscountAccountId");
                builder.HasOne(t => t.PromptPaymentDiscountAccount).WithMany().HasForeignKey(t => t.PromptPaymentDiscountAccountId);

                builder.Property(p => p.PaymentTermId).HasColumnName("PaymentTermId");
                builder.HasOne(t => t.PaymentTerm).WithMany().HasForeignKey(t => t.PaymentTermId);

                builder.Property(p => p.CustomerAdvancesAccountId).HasColumnName("CustomerAdvancesAccountId");
                builder.HasOne(t => t.CustomerAdvancesAccount).WithMany().HasForeignKey(t => t.CustomerAdvancesAccountId);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
