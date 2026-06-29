using ShoeStore.Shared.DTOs.Categories;

namespace ShoeStore.API.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
}