using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusinessCore.Domain.Items;

namespace BusinessCore.Mapping.Items
{
    public partial class ModelMap : BaseEntityMap<Model>
    {
        public override void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.ToTable("Model");

            TableColumns = () =>
            {
                builder.Property(p => p.Code).HasColumnName("Code").HasMaxLength(20);
                builder.Property(p => p.Name).HasColumnName("Name").HasMaxLength(200);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
