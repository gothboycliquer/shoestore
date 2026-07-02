using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoeStore.Data.Configurations;
public class ManufacturerConfiguration : IEntityTypeConfiguration<Manufacturer>
{
    public void Configure(EntityTypeBuilder<Manufacturer> builder)
    {
        builder.ToTable("Manufacturers");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasData(
            new Manufacturer { Id = 1, Name = "Nike" },
            new Manufacturer { Id = 2, Name = "Adidas" },
            new Manufacturer { Id = 3, Name = "Balenciaga" },
            new Manufacturer { Id = 4, Name = "New Rock" },
            new Manufacturer { Id = 5, Name = "Buffalo" },
            new Manufacturer { Id = 6, Name = "Puma" },
            new Manufacturer { Id = 7, Name = "Reebok" },
            new Manufacturer { Id = 8, Name = "New Balance" },
            new Manufacturer { Id = 9, Name = "Dr. Martens" },
            new Manufacturer { Id = 10, Name = "Converse" }
        );
    }
}