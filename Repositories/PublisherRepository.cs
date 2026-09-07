using GameStoreApi.Data;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private readonly GameStoreDbContext _dbContext;

    public PublisherRepository(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Publisher>> GetAllAsync()
    {
        return await _dbContext.Publishers
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Publisher?> GetByIdAsync(int id)
    {
        return await _dbContext.Publishers
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Publisher> CreateAsync(Publisher publisher)
    {
        await _dbContext.Publishers.AddAsync(publisher);
        await _dbContext.SaveChangesAsync();
        return publisher;
    }

    public async Task<Publisher> UpdateAsync(Publisher publisher)
    {
        _dbContext.Publishers.Update(publisher);
        await _dbContext.SaveChangesAsync();
        return publisher;
    }

    public async Task DeleteAsync(int id)
    {
        var publisher = await _dbContext.Publishers.FindAsync(id);
        if (publisher != null)
        {
            _dbContext.Publishers.Remove(publisher);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbContext.Publishers.AnyAsync(p => p.Id == id);
    }

    public async Task<bool> IsNameExistsAsync(string name, int? excludeId = null)
    {
        return await _dbContext.Publishers
            .AnyAsync(p => p.Name.ToLower() == name.ToLower() && (!excludeId.HasValue || p.Id != excludeId.Value));
    }
}