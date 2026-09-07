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

    public async Task<ServiceResult<RegisterResponseDto>> RegisterAsync(RegisterDto dto)
    {
        if (await _userRepository.ExistsAsync(dto.Username))
        {
            return ServiceResult<RegisterResponseDto>.Failure("Username is already taken.");
        }

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        await _userRepository.CreateAsync(user);

        var token = GenerateJwtToken(user);
        var response = new RegisterResponseDto(user.Username, user.Email);

        return ServiceResult<RegisterResponseDto>.Success(response, "User registered successfully.");
    }

    public async Task<ServiceResult<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return ServiceResult<AuthResponseDto>.Failure("Invalid username or password.");
        }

        var token = GenerateJwtToken(user);
        var response = new AuthResponseDto(token, user.Username);

        return ServiceResult<AuthResponseDto>.Success(response, "Login successful.");
    }

    public async Task<ServiceResult<UserProfileDto>> GetUserProfileAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return ServiceResult<UserProfileDto>.Failure("User not found.");
        }

        var profileDto = new UserProfileDto(user.Id, user.Email, user.Username);
        return ServiceResult<UserProfileDto>.Success(profileDto, "User profile retrieved successfully.");
    }

    public async Task<ServiceResult<bool>> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return ServiceResult<bool>.Failure("User not found.");
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, user.PasswordHash))
        {
            return ServiceResult<bool>.Failure("Current password is incorrect.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        await _userRepository.UpdateAsync(user);

        return ServiceResult<bool>.Success(true, "Password changed successfully.");
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