namespace ShoeStore.Data.Repositories.Interfaces;
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithDetailsAsync();
    Task<Product?> GetByIdWithDetailsAsync(int id);
    Task<bool> IsProductInOrdersAsync(int productId);
}