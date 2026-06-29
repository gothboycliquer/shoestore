using ShoeStore.Shared.DTOs.Suppliers;

namespace ShoeStore.API.Services.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
}