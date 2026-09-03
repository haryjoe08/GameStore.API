using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameResponseDto>> GetAllGamesAsync();
    Task<GameResponseDto?> GetGameByIdAsync(int id);
    Task<GameResponseDto?> CreateGameAsync(CreateGameDto dto);
    Task<GameResponseDto?> UpdateGameAsync(int id, CreateGameDto dto);
    Task<bool> DeleteGameAsync(int id);
}