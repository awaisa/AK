using BusinessCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping
{
    public partial class ContactMap : BaseEntityMap<Contact>
    {
        public override void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contact");

            TableColumns = () =>
            {
                builder.Property(p => p.ContactType).HasColumnName("ContactType");
                builder.Property(p => p.FirstName).HasColumnName("FirstName");
                builder.Property(p => p.LastName).HasColumnName("LastName");
                builder.Property(p => p.MiddleName).HasColumnName("MiddleName");

                builder.Property(p => p.PartyId).HasColumnName("PartyId");
                builder.HasOne(t => t.Party).WithMany().HasForeignKey(t => t.PartyId);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
