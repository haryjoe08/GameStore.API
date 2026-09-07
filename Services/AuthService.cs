using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using GameStoreApi.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace GameStoreApi.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepository.ExistsAsync(dto.Username))
        {
            return ApiResponse<AuthResponseDto>.Failure("Username is already taken.");
        }

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _userRepository.CreateAsync(user);

        var token = GenerateJwtToken(user);
        var response = new AuthResponseDto(token, user.Username);

        return ApiResponse<AuthResponseDto>.Success(response, "User registered successfully.");
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return ApiResponse<AuthResponseDto>.Failure("Invalid username or password.");
        }

        var token = GenerateJwtToken(user);
        var response = new AuthResponseDto(token, user.Username);

        return ApiResponse<AuthResponseDto>.Success(response, "Login successful.");
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var secretKey = jwtSettings["Secret"]
                        ?? throw new InvalidOperationException("JWT Secret is not configured.");

        var key = Encoding.UTF8.GetBytes(secretKey);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),

            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],

            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}