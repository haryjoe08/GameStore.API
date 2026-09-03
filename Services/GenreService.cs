using GameStoreApi.Data;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using GameStoreApi.Services.Interfaces;

namespace GameStoreApi.Services;

public class GenreService : IGenreService
{
    private readonly GameStoreDbContext _dbContext;

    public GenreService(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<GenreResponseDto>> GetAllAsync()
    {
        return await _dbContext.Genres
            .Select(g => new GenreResponseDto
            {
                Id = g.Id,
                Name = g.Name
            })
            .ToListAsync();
    }

    public async Task<GenreResponseDto?> GetByIdAsync(int id)
    {
        var genre = await _dbContext.Genres.FindAsync(id);
        if (genre == null) return null;

        return new GenreResponseDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }

    public async Task<GenreResponseDto> CreateAsync(CreateGenreDto dto)
    {
        var genre = new Genre
        {
            Name = dto.Name
        };

        _dbContext.Genres.Add(genre);
        await _dbContext.SaveChangesAsync();

        return new GenreResponseDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }

    public async Task<GenreResponseDto?> UpdateAsync(int id, CreateGenreDto dto)
    {
        var genre = await _dbContext.Genres.FindAsync(id);
        if (genre == null) return null;

        genre.Name = dto.Name;
        await _dbContext.SaveChangesAsync();

        return new GenreResponseDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        var genre = await _dbContext.Genres.FindAsync(id);
        if (genre == null) return false;

        // Cek apakah genre ini sedang dipakai oleh setidaknya 1 Game
        var isUsed = await _dbContext.Games.AnyAsync(g => g.GenreId == id);
        if (isUsed)
        {
            return false; 
        }

        _dbContext.Genres.Remove(genre);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}