using BusinessCore.Domain.Financials;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BusinessCore.Mapping.Financials
{
    public partial class AccountMap : BaseEntityMap<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");

            TableColumns = () =>
            {
                builder.Property(p => p.AccountClassId).HasColumnName("AccountClassId");
                builder.HasOne(t => t.AccountClass).WithMany(t => t.Accounts).HasForeignKey(t => t.AccountClassId);
                builder.Property(p => p.ParentAccountId).HasColumnName("ParentAccountId");
                builder.HasOne(t => t.ParentAccount).WithMany().HasForeignKey(t => t.ParentAccountId);

                builder.Property(p => p.DrOrCrSide).HasColumnName("DrOrCrSide");
                builder.Property(p => p.AccountCode).HasColumnName("AccountCode").HasMaxLength(50);
                builder.Property(p => p.AccountName).HasColumnName("AccountName").HasMaxLength(200);
                builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(500);
                builder.Property(p => p.IsCash).HasColumnName("IsCash");
                builder.Property(p => p.IsContraAccount).HasColumnName("IsContraAccount");
                builder.Property(p => p.RowVersion).HasColumnName("RowVersion").IsRowVersion();

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
