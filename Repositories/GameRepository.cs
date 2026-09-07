using GameStoreApi.Data;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Repositories;

public class GameRepository : IGameRepository
{
    private readonly GameStoreDbContext _dbContext;

    public GameRepository(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Game>> GetAllAsync()
    {
        return await _dbContext.Games
            .Include(g => g.Publisher)
            .Include(g => g.Genre)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Game?> GetByIdAsync(int id)
    {
        return await _dbContext.Games
            .Include(g => g.Publisher)
            .Include(g => g.Genre)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Game> CreateAsync(Game game)
    {
        await _dbContext.Games.AddAsync(game);
        await _dbContext.SaveChangesAsync();

        // Load ulang data Publisher & Genre agar Navigation Property terisi saat dikembalikan
        await _dbContext.Entry(game).Reference(g => g.Publisher).LoadAsync();
        await _dbContext.Entry(game).Reference(g => g.Genre).LoadAsync();

        return game;
    }

    public async Task<Game> UpdateAsync(Game game)
    {
        _dbContext.Games.Update(game);
        await _dbContext.SaveChangesAsync();

        // Refresh Navigation Property
        await _dbContext.Entry(game).Reference(g => g.Publisher).LoadAsync();
        await _dbContext.Entry(game).Reference(g => g.Genre).LoadAsync();

        return game;
    }

    public async Task DeleteAsync(int id)
    {
        var game = await _dbContext.Games.FindAsync(id);
        if (game != null)
        {
            _dbContext.Games.Remove(game);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbContext.Games.AnyAsync(g => g.Id == id);
    }

    
}