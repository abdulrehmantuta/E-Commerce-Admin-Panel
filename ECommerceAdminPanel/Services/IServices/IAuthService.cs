using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.DTOs.Response;

namespace ECommerceAdminPanel.Services.IServices;

public interface IAuthService
{
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request);
}