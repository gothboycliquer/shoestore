using ShoeStore.Data.Repositories.Interfaces;

namespace ShoeStore.Data.Repositories;

public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
{
    public ManufacturerRepository(ApplicationDbContext context) : base(context)
    {
    }
}