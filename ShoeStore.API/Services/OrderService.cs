using AutoMapper;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.Data.Repositories.Interfaces;
using ShoeStore.Shared.DTOs.Orders;

namespace ShoeStore.API.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _unitOfWork.Orders.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);
        return order == null ? null : _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        var order = _mapper.Map<ShoeStore.Data.Order>(dto);
        order.CreatedAt = DateTime.UtcNow;
        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Orders.GetByIdWithDetailsAsync(order.Id);
        return _mapper.Map<OrderDto>(created!);
    }

    public async Task<OrderDto?> UpdateAsync(int id, CreateOrderDto dto)
    {
        var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);
        if (order == null)
            return null;

        order.Status = dto.Status;

        await _unitOfWork.Orders.DeleteOrderItemsAsync(id);
        await _unitOfWork.SaveChangesAsync();

        foreach (var itemDto in dto.OrderItems)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
            if (product != null)
            {
                order.OrderItems.Add(new ShoeStore.Data.OrderItem
                {
                    OrderId = id,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    PriceAtPurchase = product.Price
                });
            }
        }

        await _unitOfWork.Orders.UpdateAsync(order);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Orders.GetByIdWithDetailsAsync(id);
        return _mapper.Map<OrderDto>(updated!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null)
            return false;

        await _unitOfWork.Orders.DeleteAsync(order);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}