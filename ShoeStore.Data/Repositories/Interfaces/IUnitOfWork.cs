namespace ShoeStore.Data.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    ICategoryRepository Categories { get; }
    IManufacturerRepository Manufacturers { get; }
    ISupplierRepository Suppliers { get; }
    Task<int> SaveChangesAsync();
}