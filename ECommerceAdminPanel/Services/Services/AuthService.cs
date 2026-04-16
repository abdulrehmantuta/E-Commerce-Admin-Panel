using ECommerceAdminPanel.DTOs.Request;
using ECommerceAdminPanel.DTOs.Response;
using ECommerceAdminPanel.Repositories.IRepository;
using ECommerceAdminPanel.Services.IServices;
using ECommerceAdminPanel.Models;   // ✅
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;  

namespace ECommerceAdminPanel.Services.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        try
        {
            var email = request.Email?.Trim();
            if (string.IsNullOrWhiteSpace(email))
                return ApiResponse<LoginResponseDto>.ErrorResponse("Email is required", 400);

            if (string.IsNullOrWhiteSpace(request.Password))
                return ApiResponse<LoginResponseDto>.ErrorResponse("Password is required", 400);

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return ApiResponse<LoginResponseDto>.ErrorResponse("Invalid email or password", 401);

            if (!user.Status)
                return ApiResponse<LoginResponseDto>.ErrorResponse("Account is inactive. Please contact support.", 403);

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                return ApiResponse<LoginResponseDto>.ErrorResponse("Invalid email or password", 401);

            var token = GenerateJwtToken(user);

            var response = new LoginResponseDto
            {
                Token = token,
                UserId = user.UserId,
                TenantId = user.TenantId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return ApiResponse<LoginResponseDto>.SuccessResponse(response, "Login successful");
        }
        catch (Exception ex)
        {
            return ApiResponse<LoginResponseDto>.ErrorResponse($"Login error: {ex.Message}", 500);
        }
    }

    // private string GenerateJwtToken(User user)
    private string GenerateJwtToken(ECommerceAdminPanel.Models.User user)
    {
        var jwtKey = _configuration["Jwt:Key"]!;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("TenantId", user.TenantId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}