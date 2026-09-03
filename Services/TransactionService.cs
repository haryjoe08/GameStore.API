using GameStoreApi.Data;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using GameStoreApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Services;

public class TransactionService : ITransactionService
{
    private readonly GameStoreDbContext _dbContext;

    public TransactionService(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TransactionResponseDto?>> GetAllAsync()
    {
        return await _dbContext.Transactions
            .Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                GameId = t.GameId,
                GameTitle = t.Game != null ? t.Game.Title : "N/A",
                Quantity = t.Quantity,
                TotalPrice = t.TotalPrice,
                TransactionDate = t.TransactionDate
            })
            .ToListAsync();
    }

    public async Task<TransactionResponseDto?> GetByIdAsync(int id)
    {
        return await _dbContext.Transactions
            .Where(t => t.Id == id)
            .Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                GameId = t.GameId,
                GameTitle = t.Game != null ? t.Game.Title : "N/A",
                Quantity = t.Quantity,
                TotalPrice = t.TotalPrice,
                TransactionDate = t.TransactionDate
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TransactionResponseDto?> CreateAsync(CreateTransactionDto dto)
    {
        var game = await _dbContext.Games.FindAsync(dto.GameId);
        
        if (game == null || game.Stock < dto.Quantity)
        {
            return null;
        }
        
        game.Stock -= dto.Quantity;

        decimal totalPrice = game.Price * dto.Quantity;
        
        var transaction = new Transaction
        {
            GameId = dto.GameId,
            Quantity = dto.Quantity,
            TotalPrice = totalPrice,
            TransactionDate = DateTime.UtcNow
        };
        
        _dbContext.Transactions.Add(transaction);
        await _dbContext.SaveChangesAsync();
        
        return await GetByIdAsync(transaction.Id);
    }
}