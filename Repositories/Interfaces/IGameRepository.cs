using GameStoreApi.DTOs;
using GameStoreApi.Models;

namespace GameStoreApi.Repositories.Interfaces;

public interface  IGameRepository
{
    Task<List<Game>> GetAllAsync();
    Task<Game?> GetByIdAsync(int id);
    Task<Game> CreateAsync(Game game);
    Task<Game> UpdateAsync(Game game);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}