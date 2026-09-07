using GameStoreApi.Services.Interfaces;
using GameStoreApi.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _authService.RegisterAsync(registerDto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        
        return Ok(result);
    }
    
    
    [HttpPost(" login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _authService.LoginAsync(loginDto);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}