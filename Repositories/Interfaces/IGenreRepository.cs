using GameStoreApi.Models;

namespace GameStoreApi.Repositories.Interfaces;

public interface IGenreRepository
{
    Task<List<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(int id);
    Task<Genre> CreateAsync(Genre genre);
    Task<Genre> UpdateAsync(Genre genre);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
}