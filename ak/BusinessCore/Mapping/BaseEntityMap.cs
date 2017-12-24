using BusinessCore.Domain;
using BusinessCore.Domain.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessCore.Mapping
{
    public abstract partial class BaseEntityMap<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        Action _tableColumns;

        public Action TableColumns { get => _tableColumns; set => _tableColumns = value; }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id");

            if (_tableColumns != null)
                TableColumns();

            builder.Property(p => p.CreatedOn).HasColumnName("CreatedOn");
            builder.Property(p => p.CreatedById).HasColumnName("CreatedById");
            builder.Property(p => p.ModifiedOn).HasColumnName("ModifiedOn");
            builder.Property(p => p.ModifiedById).HasColumnName("ModifiedById");
            builder.Property(p => p.IsActive).HasColumnName("IsInactive");
        }
    }
}
