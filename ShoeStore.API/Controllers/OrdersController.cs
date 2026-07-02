using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.Shared.DTOs.Orders;
using ShoeStore.Shared.Helpers;

namespace ShoeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "ManagerOrAdmin")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<OrderDto>>.Ok(orders));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetById(int id)
    {
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
            return NotFound(ApiResponse<OrderDto>.Fail("Заказ не найден."));

        return Ok(ApiResponse<OrderDto>.Ok(order));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> Create([FromBody] CreateOrderDto dto)
    {
        var order = await _orderService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = order.Id },
            ApiResponse<OrderDto>.Ok(order, "Заказ успешно создан."));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> Update(int id, [FromBody] CreateOrderDto dto)
    {
        var order = await _orderService.UpdateAsync(id, dto);
        if (order == null)
            return NotFound(ApiResponse<OrderDto>.Fail("Заказ не найден."));

        return Ok(ApiResponse<OrderDto>.Ok(order, "Заказ успешно обновлён."));
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _orderService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<bool>.Fail("Заказ не найден."));

        return Ok(ApiResponse<bool>.Ok(true, "Заказ успешно удалён."));
    }
}