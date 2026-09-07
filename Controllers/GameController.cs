using GameStoreApi.DTOs;
using GameStoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var games = await _gameService.GetAllGamesAsync();
        return Ok(games);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var game = await _gameService.GetGameByIdAsync(id);
        
        return Ok(game);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateGameDto dto)
    {
        var result = await _gameService.CreateGameAsync(dto);
        if (!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, CreateGameDto dto)
    {
        var result = await _gameService.UpdateGameAsync(id, dto);
        if (!result.IsSuccess) return BadRequest(result);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _gameService.DeleteGameAsync(id);
        if (!result.IsSuccess) return BadRequest(result);

        return Ok(result);
    }
}