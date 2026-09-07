namespace GameStoreApi.DTOs;

public record RegisterDto(string Username, string Email, string Password);
public record LoginDto(string Username, string Password);
public record RegisterResponseDto(string Username, string Email);
public record AuthResponseDto(string Token, string Username);
public record UserProfileDto(int Id, string Email, string Username);
public record ChangePasswordDto(string CurrentPassword, string NewPassword);
