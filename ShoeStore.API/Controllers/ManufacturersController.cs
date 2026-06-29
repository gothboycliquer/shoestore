using Microsoft.AspNetCore.Mvc;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.Shared.DTOs.Manufacturers;
using ShoeStore.Shared.Helpers;

namespace ShoeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManufacturersController : ControllerBase
{
    private readonly IManufacturerService _manufacturerService;

    public ManufacturersController(IManufacturerService manufacturerService)
    {
        _manufacturerService = manufacturerService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ManufacturerDto>>>> GetAll()
    {
        var manufacturers = await _manufacturerService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<ManufacturerDto>>.Ok(manufacturers));
    }
}