using Microsoft.EntityFrameworkCore;
using ShoeStore.Data.Repositories.Interfaces;

namespace ShoeStore.Data.Repositories;
public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Manufacturer)
            .Include(p => p.Supplier)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Manufacturer)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> IsProductInOrdersAsync(int productId)
    {
        return await _context.OrderItems
            .AnyAsync(oi => oi.ProductId == productId);
    }
}