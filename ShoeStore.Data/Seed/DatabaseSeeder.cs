using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ShoeStore.Data.Seed;
public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
    {
        try
        {
            await SeedUsersAsync(context);
            await SeedProductsAsync(context);
            await context.SaveChangesAsync();
            logger.LogInformation("База данных успешно заполнена начальными данными.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при заполнении базы данных.");
            throw;
        }
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var users = new List<User>
        {
            new User
            {
                Login = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                FullName = "Главный Администратор",
                RoleId = 4
            },
            new User
            {
                Login = "manager",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"),
                FullName = "Иванов Иван Иванович",
                RoleId = 3
            },
            new User
            {
                Login = "client",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("client123"),
                FullName = "Петров Пётр Петрович",
                RoleId = 2
            }
        };

        await context.Users.AddRangeAsync(users);
    }

    private static async Task SeedProductsAsync(ApplicationDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var products = new List<Product>
        {
            new Product
            {
                Name = "Nike Air Max 90",
                Description = "Классические кроссовки с технологией Air для максимального комфорта.",
                Price = 8990,
                Unit = "пара",
                Quantity = 15,
                Discount = 0,
                CategoryId = 1,
                ManufacturerId = 1,
                SupplierId = 1
            },
            new Product
            {
                Name = "Adidas Ultraboost 22",
                Description = "Беговые кроссовки с адаптивной подошвой Boost.",
                Price = 12500,
                Unit = "пара",
                Quantity = 8,
                Discount = 10,
                CategoryId = 1,
                ManufacturerId = 2,
                SupplierId = 2
            },
            new Product
            {
                Name = "Balenciaga Track",
                Description = "Массивные кроссовки с многослойной подошвой в стиле dad shoes.",
                Price = 89000,
                Unit = "пара",
                Quantity = 3,
                Discount = 0,
                CategoryId = 1,
                ManufacturerId = 3,
                SupplierId = 3
            },
            new Product
            {
                Name = "New Rock M.WALL083",
                Description = "Культовые ботинки в готическом стиле с массивной подошвой и металлическими деталями.",
                Price = 24500,
                Unit = "пара",
                Quantity = 5,
                Discount = 0,
                CategoryId = 4,
                ManufacturerId = 4,
                SupplierId = 1
            },
            new Product
            {
                Name = "Buffalo 1339-14",
                Description = "Легендарные платформенные кроссовки 90-х с массивной подошвой.",
                Price = 18900,
                Unit = "пара",
                Quantity = 4,
                Discount = 15,
                CategoryId = 1,
                ManufacturerId = 5,
                SupplierId = 2
            },
            new Product
            {
                Name = "Nike Air Jordan 1 Retro",
                Description = "Культовые баскетбольные кроссовки ставшие иконой уличной моды.",
                Price = 15000,
                Unit = "пара",
                Quantity = 6,
                Discount = 0,
                CategoryId = 1,
                ManufacturerId = 1,
                SupplierId = 3
            },
            new Product
            {
                Name = "Adidas Samba OG",
                Description = "Футбольные кроссовки адаптированные для города.",
                Price = 8200,
                Unit = "пара",
                Quantity = 0,
                Discount = 0,
                CategoryId = 1,
                ManufacturerId = 2,
                SupplierId = 1
            },
            new Product
            {
                Name = "Puma Suede Classic",
                Description = "Легендарные замшевые кроссовки в городском стиле.",
                Price = 6500,
                Unit = "пара",
                Quantity = 20,
                Discount = 20,
                CategoryId = 1,
                ManufacturerId = 6,
                SupplierId = 2
            },
            new Product
            {
                Name = "Reebok Classic Leather",
                Description = "Культовые кожаные кроссовки для повседневной носки.",
                Price = 5800,
                Unit = "пара",
                Quantity = 12,
                Discount = 0,
                CategoryId = 1,
                ManufacturerId = 7,
                SupplierId = 3
            },
            new Product
            {
                Name = "New Balance 574",
                Description = "Удобные кроссовки с замшевыми вставками.",
                Price = 7900,
                Unit = "пара",
                Quantity = 3,
                Discount = 18,
                CategoryId = 1,
                ManufacturerId = 8,
                SupplierId = 1
            },
            new Product
            {
                Name = "Dr. Martens 1460",
                Description = "Классические ботинки на шнуровке с фирменной желтой строчкой.",
                Price = 16500,
                Unit = "пара",
                Quantity = 9,
                Discount = 5,
                CategoryId = 4,
                ManufacturerId = 9,
                SupplierId = 2
            },
            new Product
            {
                Name = "Converse Chuck Taylor All Star",
                Description = "Парусиновые кеды ставшие символом целого поколения.",
                Price = 4900,
                Unit = "пара",
                Quantity = 25,
                Discount = 0,
                CategoryId = 1,
                ManufacturerId = 10,
                SupplierId = 3
            }
        };
        await context.Products.AddRangeAsync(products);
    }
}