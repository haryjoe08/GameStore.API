using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IGameService
{
    Task<ApiResponse<IEnumerable<GameResponseDto>>> GetAllGamesAsync();
    Task<ApiResponse<GameResponseDto>> GetGameByIdAsync(int id);
    Task<ApiResponse<GameResponseDto>>CreateGameAsync(CreateGameDto dto);
    Task<ApiResponse<GameResponseDto>> UpdateGameAsync(int id, CreateGameDto dto);
    Task<ApiResponse<bool>> DeleteGameAsync(int id);
}