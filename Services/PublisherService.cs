using GameStoreApi.Data;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using GameStoreApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Services;

public class PublisherService : IPublisherService
{
    private readonly GameStoreDbContext _dbContext;

    public PublisherService(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<PublisherResponseDto>> GetAllAsync()
    {
        return await _dbContext.Publishers
            .Select(p => new PublisherResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Country = p.Country
            })
            .ToListAsync();
    }

    public async Task<PublisherResponseDto?> GetByIdAsync(int id)
    {
        var publisher = await _dbContext.Publishers.FindAsync(id);
        if (publisher == null) return null;

        return new PublisherResponseDto
        {
            Id = publisher.Id,
            Name = publisher.Name,
            Country = publisher.Country
        };
    }

    public async Task<PublisherResponseDto> CreateAsync(CreatePublisherDto dto)
    {
        var publisher = new Publisher
        {
            Name = dto.Name,
            Country = dto.Country
        };

        _dbContext.Publishers.Add(publisher);
        await _dbContext.SaveChangesAsync();

        return new PublisherResponseDto
        {
            Id = publisher.Id,
            Name = publisher.Name,
            Country = publisher.Country
        };
    }

    public async Task<PublisherResponseDto?> UpdateAsync(int id, CreatePublisherDto dto)
    {
        var publisher = await _dbContext.Publishers.FindAsync(id);
        if (publisher == null) return null;

        publisher.Name = dto.Name;
        publisher.Country = dto.Country;

        await _dbContext.SaveChangesAsync();

        return new PublisherResponseDto
        {
            Id = publisher.Id,
            Name = publisher.Name,
            Country = publisher.Country
        };
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        var publisher = await _dbContext.Publishers.FindAsync(id);
        if (publisher == null) return null; 

        // Cek apakah publisher ini masih dipakai di tabel Game
        var isUsed = await _dbContext.Games.AnyAsync(g => g.PublisherId == id);
        if (isUsed)
        {
            return false; 
        }

        _dbContext.Publishers.Remove(publisher);
        await _dbContext.SaveChangesAsync();
        return true; 
    }
}