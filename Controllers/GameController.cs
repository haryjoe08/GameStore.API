using GameStoreApi.DTOs;
using GameStoreApi.Services.Interfaces;
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
    public async Task<IActionResult> GetAll()
    {
        var games = await _gameService.GetAllGamesAsync();
        return Ok(games);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var game = await _gameService.GetGameByIdAsync(id);
        
        if (game == null)
        {
            return NotFound(new { Message = "Game tidak ditemukan." });
        }
        return Ok(game);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGameDto dto)
    {
        var createdGame = await _gameService.CreateGameAsync(dto);

        if (createdGame == null)
        {
            return BadRequest(new { Message = "PublisherId atau GenreId tidak valid." });
        }

        return Ok(new
        {
            Message = "Game berhasil ditambahkan!",
            Data = createdGame
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateGameDto dto)
    {
        var updatedGame = await _gameService.UpdateGameAsync(id, dto);
        if (updatedGame == null)
        {
            return NotFound(new { Message = "Game tidak ditemukan atau Publisher/Genre ID tidak valid." });
        }

        return Ok(new
        {
            Message = "Game berhasil diperbarui!",
            Data = updatedGame
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _gameService.DeleteGameAsync(id);
        if (!success) return NotFound("Game tidak ditemukan.");

        return NoContent();
    }
}