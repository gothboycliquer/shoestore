using Microsoft.EntityFrameworkCore;
using ShoeStore.Data.Repositories.Interfaces;

namespace ShoeStore.Data.Repositories;
public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }
        public async Task DeleteOrderItemsAsync(int orderId)
    {
        var items = _context.OrderItems.Where(oi => oi.OrderId == orderId);
        _context.OrderItems.RemoveRange(items);
        await Task.CompletedTask;
    }
}