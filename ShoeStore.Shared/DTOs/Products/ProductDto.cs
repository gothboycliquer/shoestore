namespace ShoeStore.Shared.DTOs.Products;
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Unit { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
    public string? ImagePath { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string ManufacturerName { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int ManufacturerId { get; set; }
    public int SupplierId { get; set; }
}