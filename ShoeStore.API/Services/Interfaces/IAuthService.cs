using ShoeStore.Shared.DTOs.Auth;

namespace ShoeStore.API.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}