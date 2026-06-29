using ShoeStore.Data.Repositories.Interfaces;

namespace ShoeStore.Data.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}