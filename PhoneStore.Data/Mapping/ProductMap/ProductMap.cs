using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneStore.Core.Domain;

namespace PhoneStore.Data.Mapping.ProductMap
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).HasMaxLength(500);
            builder.Property(p => p.OperatingSystem).HasMaxLength(500);
            builder.Property(p => p.Price).HasColumnType("decimal(18,4)");
            builder.Property(p => p.Processor).HasMaxLength(500); 
            builder.Property(p => p.ScreenResolution).HasColumnType("decimal(18,4)"); 
            builder.Property(p => p.Size).HasMaxLength(100); 
            builder.Property(p => p.Storage).HasMaxLength(100); 
            builder.Property(p => p.VideoLink).HasMaxLength(10000);

            builder.HasMany(p => p.Pictures).WithOne(x => x.Product).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
