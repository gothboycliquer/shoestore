using ShoeStore.Shared.DTOs.Manufacturers;

namespace ShoeStore.API.Services.Interfaces;

public interface IManufacturerService
{
    Task<IEnumerable<ManufacturerDto>> GetAllAsync();
}