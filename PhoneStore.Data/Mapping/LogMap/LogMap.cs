using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStore.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Data.Mapping.LogMap
{
    public class LogMap : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("Logs");
            builder.Property(l => l.Request).HasMaxLength(int.MaxValue);
            builder.Property(l => l.RequestInformation).HasMaxLength(int.MaxValue);
            builder.Property(p => p.Response).HasMaxLength(int.MaxValue);
            builder.Property(p => p.ResponseInformation).HasMaxLength(int.MaxValue);
            builder.Property(p => p.IpAddress).HasMaxLength(1000);
        }
    }
}
