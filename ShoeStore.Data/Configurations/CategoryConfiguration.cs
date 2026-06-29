using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoeStore.Data.Configurations;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasData(
            new Category { Id = 1, Name = "Кроссовки" },
            new Category { Id = 2, Name = "Туфли" },
            new Category { Id = 3, Name = "Сапоги" },
            new Category { Id = 4, Name = "Ботинки" },
            new Category { Id = 5, Name = "Сандалии" }
        );
    }
}