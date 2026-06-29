using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoeStore.Data.Configurations;
public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Suppliers");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasData(
            new Supplier { Id = 1, Name = "Fujian ShoeGroup Co." },
            new Supplier { Id = 2, Name = "Guangzhou FootWear Ltd." },
            new Supplier { Id = 3, Name = "Shenzhen SoleMax Trading" }
        );
    }
}