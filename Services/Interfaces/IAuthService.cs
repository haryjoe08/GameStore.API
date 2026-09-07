using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IAuthService
{
    Task<ServiceResult<RegisterResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto);
    Task<ServiceResult<UserProfileDto>> GetUserProfileAsync(int userId);
    Task<ServiceResult<bool>> ChangePasswordAsync(int userId, ChangePasswordDto dto); 
}