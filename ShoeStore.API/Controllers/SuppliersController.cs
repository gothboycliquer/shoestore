using Microsoft.AspNetCore.Mvc;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.Shared.DTOs.Suppliers;
using ShoeStore.Shared.Helpers;

namespace ShoeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<SupplierDto>>>> GetAll()
    {
        var suppliers = await _supplierService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<SupplierDto>>.Ok(suppliers));
    }
}