using ShoeStore.Data.Repositories.Interfaces;

namespace ShoeStore.Data.Repositories;

public class SupplierRepository : Repository<Supplier>, ISupplierRepository
{
    public SupplierRepository(ApplicationDbContext context) : base(context)
    {
    }
}