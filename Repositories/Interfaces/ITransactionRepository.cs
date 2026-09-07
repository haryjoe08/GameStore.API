using GameStoreApi.Models;

namespace GameStoreApi.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetAllAsync();
    Task<Transaction?> GetByIdAsync(int id);
    Task<Transaction> CreateAsync(Transaction transaction);
    Task<bool> ExistsAsync(int id);
}