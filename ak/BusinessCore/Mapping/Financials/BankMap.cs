
using BusinessCore.Domain.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Financials
{
    public partial class BankMap : BaseEntityMap<Bank>
    {
        public override void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.ToTable("Bank");

            TableColumns = () =>
            {
                builder.Property(p => p.Type).HasColumnName("Type");
                builder.Property(p => p.Name).HasColumnName("Name");
                builder.Property(p => p.AccountId).HasColumnName("AccountId");
                builder.HasOne(t => t.Account).WithMany().HasForeignKey(t => t.AccountId);
                builder.Property(p => p.BankName).HasColumnName("BankName");
                builder.Property(p => p.Number).HasColumnName("Number");
                builder.Property(p => p.Address).HasColumnName("Address");
                builder.Property(p => p.IsDefault).HasColumnName("IsDefault");
                //builder.Property(p => p.IsActive).HasColumnName("IsActive");

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }

    }
}
