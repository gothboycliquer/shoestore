namespace ShoeStore.Shared.DTOs.Orders;
public class CreateOrderDto
{
    public int UserId { get; set; }
    public string Status { get; set; } = "Новый";
    public List<CreateOrderItemDto> OrderItems { get; set; } = new();
}