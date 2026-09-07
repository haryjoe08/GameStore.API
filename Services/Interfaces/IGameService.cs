using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IGameService
{
    Task<ServiceResult<IEnumerable<GameResponseDto>>> GetAllGamesAsync();
    Task<ServiceResult<GameResponseDto>> GetGameByIdAsync(int id);
    Task<ServiceResult<GameResponseDto>>CreateGameAsync(CreateGameDto dto);
    Task<ServiceResult<GameResponseDto>> UpdateGameAsync(int id, CreateGameDto dto);
    Task<ServiceResult<bool>> DeleteGameAsync(int id);
}