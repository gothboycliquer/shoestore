namespace ShoeStore.Data.Repositories.Interfaces;
public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetAllWithDetailsAsync();
    Task<Order?> GetByIdWithDetailsAsync(int id);
    Task DeleteOrderItemsAsync(int orderId);
}