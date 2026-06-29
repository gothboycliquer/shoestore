using ShoeStore.Shared.DTOs.Orders;

namespace ShoeStore.API.Services.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task<OrderDto> CreateAsync(CreateOrderDto dto);
    Task<OrderDto?> UpdateAsync(int id, CreateOrderDto dto);
    Task<bool> DeleteAsync(int id);
}