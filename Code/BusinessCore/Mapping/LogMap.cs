using BusinessCore.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessCore.Mapping
{
    public partial class LogMap : BaseEntityMap<Log>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Log");

            TableColumns = () =>
            {
                builder.Property(p => p.TimeStamp).HasColumnName("TimeStamp");
                builder.Property(p => p.Level).HasColumnName("Level");
                builder.Property(p => p.Logger).HasColumnName("Logger");
                builder.Property(p => p.Message).HasColumnName("Message");

                builder.Property(p => p.Username).HasColumnName("Username").HasMaxLength(100);
                builder.Property(p => p.CallSite).HasColumnName("CallSite").HasMaxLength(100);
                builder.Property(p => p.Thread).HasColumnName("Thread").HasMaxLength(100);
                builder.Property(p => p.Exception).HasColumnName("Exception");
                builder.Property(p => p.StackTrace).HasColumnName("StackTrace");
            };

            base.Configure(builder);
        }
    }
}
