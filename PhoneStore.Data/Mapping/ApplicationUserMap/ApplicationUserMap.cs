using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStore.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Data.Mapping.ApplicationUserMap
{
    public class ApplicationUserMap : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(u => u.Products).WithOne(u => u.User).HasForeignKey(x => x.UserId);
        }
    }
}
