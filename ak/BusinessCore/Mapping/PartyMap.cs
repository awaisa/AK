using BusinessCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping
{
    public partial class PartyMap : BaseEntityMap<Party>
    {
        public override void Configure(EntityTypeBuilder<Party> builder)
        {
            builder.ToTable("Party");

            TableColumns = () =>
            {
                builder.Property(p => p.PartyType).HasColumnName("PartyType");
                builder.Property(p => p.Name).HasColumnName("Name");
                builder.Property(p => p.Email).HasColumnName("Email");
                builder.Property(p => p.Website).HasColumnName("Website");
                builder.Property(p => p.Phone).HasColumnName("Phone");
                builder.Property(p => p.Fax).HasColumnName("Fax");
                builder.Property(p => p.Address).HasColumnName("Address");
                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
