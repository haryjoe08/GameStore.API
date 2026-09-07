using GameStoreApi.Models;

namespace GameStoreApi.Repositories.Interfaces;

public interface IPublisherRepository
{
    Task<List<Publisher>> GetAllAsync();
    Task<Publisher?> GetByIdAsync(int id);
    Task<Publisher> CreateAsync(Publisher publisher);
    Task<Publisher> UpdateAsync(Publisher publisher);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> IsNameExistsAsync(string name, int? excludeId = null);
}