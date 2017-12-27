using BusinessCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping
{
    public partial class CompanySettingMap : BaseEntityMap<CompanySetting>
    {
        public override void Configure(EntityTypeBuilder<CompanySetting> builder)
        {
            builder.ToTable("CompanySetting");

            TableColumns = () =>
            {
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");

                builder.HasOne(t => t.Company)
                        .WithMany()
                        .HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
