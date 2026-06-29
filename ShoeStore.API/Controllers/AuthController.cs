using Microsoft.AspNetCore.Mvc;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.Shared.DTOs.Auth;
using ShoeStore.Shared.Helpers;

namespace ShoeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
            return Unauthorized(ApiResponse<LoginResponseDto>.Fail("Неверный логин или пароль."));

        return Ok(ApiResponse<LoginResponseDto>.Ok(result, "Вход выполнен успешно."));
    }

    [HttpPost("guest")]
    public ActionResult<ApiResponse<LoginResponseDto>> LoginAsGuest()
    {
        var guestResponse = new LoginResponseDto
        {
            Token = string.Empty,
            FullName = "Гость",
            Role = "Гость",
            UserId = 0
        };

        return Ok(ApiResponse<LoginResponseDto>.Ok(guestResponse, "Вход как гость."));
    }
}