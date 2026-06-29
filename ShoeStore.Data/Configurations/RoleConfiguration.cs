using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShoeStore.Data.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasData(
            new Role { Id = 1, Name = "Гость" },
            new Role { Id = 2, Name = "Клиент" },
            new Role { Id = 3, Name = "Менеджер" },
            new Role { Id = 4, Name = "Администратор" }
        );
    }
}