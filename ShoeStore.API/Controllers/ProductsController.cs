using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.Shared.DTOs.Products;
using ShoeStore.Shared.Helpers;

namespace ShoeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<ProductDto>>.Ok(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
            return NotFound(ApiResponse<ProductDto>.Fail("Товар не найден."));

        return Ok(ApiResponse<ProductDto>.Ok(product));
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<ProductDto>>> Create([FromBody] CreateProductDto dto)
    {
        var product = await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id },
            ApiResponse<ProductDto>.Ok(product, "Товар успешно создан."));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<ProductDto>>> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var product = await _productService.UpdateAsync(id, dto);
        if (product == null)
            return NotFound(ApiResponse<ProductDto>.Fail("Товар не найден."));

        return Ok(ApiResponse<ProductDto>.Ok(product, "Товар успешно обновлён."));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);
        if (!result)
            return NotFound(ApiResponse<bool>.Fail("Товар не найден."));

        return Ok(ApiResponse<bool>.Ok(true, "Товар успешно удалён."));
    }
}