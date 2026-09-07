using GameStoreApi.Data;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly GameStoreDbContext _dbContext;

    public TransactionRepository(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Transaction>> GetAllAsync()
    {
        return await _dbContext.Transactions
            .Include(t => t.Game)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(int id)
    {
        return await _dbContext.Transactions
            .Include(t => t.Game)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        await _dbContext.Transactions.AddAsync(transaction);
        await _dbContext.SaveChangesAsync();

        // Load Navigation Property Game agar data GameTitle terisi pada DTO response
        await _dbContext.Entry(transaction).Reference(t => t.Game).LoadAsync();

        return transaction;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbContext.Transactions.AnyAsync(t => t.Id == id);
    }
}