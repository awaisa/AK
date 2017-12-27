using BusinessCore.Domain.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusinessCore.Mapping.Items
{
    public partial class MeasurementMap : BaseEntityMap<Measurement>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public override void Configure(EntityTypeBuilder<Measurement> builder)
        {
            builder.ToTable("Measurement");

            TableColumns = () =>
            {
                builder.Property(p => p.Code).HasColumnName("Code").HasMaxLength(20);
                builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(500);

                builder.Property(p => p.CompanyId).HasColumnName("CompanyId");
                builder.HasOne(t => t.Company).WithMany().HasForeignKey(t => t.CompanyId);
            };

            base.Configure(builder);
        }
    }
}
