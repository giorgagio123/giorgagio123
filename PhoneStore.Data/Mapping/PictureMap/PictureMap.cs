using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStore.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneStore.Data.Mapping.PictureMap
{
    public class PictureMap : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> builder)
        {
            builder.ToTable("Pictures");
            builder.Property(p => p.Filename).HasMaxLength(1000);
            builder.Property(p => p.MimeType).HasMaxLength(1000);
            builder.Property(p => p.PictureBinary).HasMaxLength(100000);

            builder.HasOne(p => p.Product).WithMany(p => p.Pictures).HasForeignKey(p => p.ProductId);
        }
    }
}
