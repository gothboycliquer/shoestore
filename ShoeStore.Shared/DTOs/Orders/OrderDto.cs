namespace ShoeStore.Shared.DTOs.Orders;
public class OrderDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;
    public int UserId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; } = new();
}