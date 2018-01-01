using BusinessCore.Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Items
{
    public partial class ItemBrandMap : BaseEntityMap<ItemBrand>
    {
        public override void Configure(EntityTypeBuilder<ItemBrand> builder)
        {
            builder.ToTable("ItemBrand");

            TableColumns = () =>
            {
                builder.Property(p => p.Code).HasColumnName("Code").HasMaxLength(20);
                builder.Property(p => p.Name).HasColumnName("Name").HasMaxLength(100);
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
