using GameStoreApi.Data;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly GameStoreDbContext _dbContext;

    public GenreRepository(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Genre>> GetAllAsync()
    {
        return await _dbContext.Genres
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        return await _dbContext.Genres
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async Task<Genre> CreateAsync(Genre genre)
    {
        await _dbContext.Genres.AddAsync(genre);
        await _dbContext.SaveChangesAsync();
        return genre;
    }

    public async Task<Genre> UpdateAsync(Genre genre)
    {
        _dbContext.Genres.Update(genre);
        await _dbContext.SaveChangesAsync();
        return genre;
    }

    public async Task DeleteAsync(int id)
    {
        var genre = await _dbContext.Genres.FindAsync(id);
        if (genre != null)
        {
            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbContext.Genres.AnyAsync(g => g.Id == id);
    }

    public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
    {
        return await _dbContext.Genres
            .AnyAsync(g => g.Name.ToLower() == name.ToLower() && (!excludeId.HasValue || g.Id != excludeId.Value));
    }
}