using BusinessCore.Domain.Financials;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Financials
{
    public class AccountClassMap : BaseEntityMap<AccountClass>
    {
        public override void Configure(EntityTypeBuilder<AccountClass> builder)
        {
            builder.ToTable("AccountClass");

            TableColumns = () =>
            {
                builder.Property(p => p.Name).HasColumnName("Name").HasMaxLength(100);
                builder.Property(p => p.NormalBalance).HasColumnName("NormalBalance").HasMaxLength(50);
            };

            base.Configure(builder);
        }
    }
}
