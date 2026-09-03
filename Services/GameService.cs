using GameStoreApi.Data;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using GameStoreApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Services;

public class GameService : IGameService
{
    private readonly GameStoreDbContext _dbContext;

    public GameService(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<GameResponseDto>> GetAllGamesAsync()
    {
        return await _dbContext.Games
            .Include(g => g.Publisher)
            .Include(g => g.Genre)
            .Select(g => new GameResponseDto
            {
                Id = g.Id,
                Title = g.Title,
                Price = g.Price,
                Stock = g.Stock,
                PublisherName = g.Publisher != null ? g.Publisher.Name : "N/A",
                GenreName = g.Genre != null ? g.Genre.Name : "N/A"
            })
            .ToListAsync();
    }

    public async Task<GameResponseDto?> CreateGameAsync(CreateGameDto dto)
    {
        var publisherExists = await _dbContext.Publishers.AnyAsync(p => p.Id == dto.PublisherId);
        var genreExists = await _dbContext.Genres.AnyAsync(g => g.Id == dto.GenreId);

        if (!publisherExists || !genreExists)
        {
            throw new KeyNotFoundException("Publisher atau Genre tidak ditemukan.");
        }

        var game = new Game
        {
            Title = dto.Title,
            Price = dto.Price,
            Stock = dto.Stock,
            PublisherId = dto.PublisherId,
            GenreId = dto.GenreId
        };

        _dbContext.Games.Add(game);
        await _dbContext.SaveChangesAsync();
        
        return await GetGameByIdAsync(game.Id);
    }

    public async Task<GameResponseDto?> GetGameByIdAsync(int id)
    {
        return await _dbContext.Games
            .Where(g => g.Id == id)
            .Select(g => new GameResponseDto
            {
                Id = g.Id,
                Title = g.Title,
                Price = g.Price,
                Stock = g.Stock,
                // EF Core otomatis paham cara INNER JOIN ke tabel Publisher & Genre
                PublisherName = g.Publisher != null ? g.Publisher.Name : "N/A",
                GenreName = g.Genre != null ? g.Genre.Name : "N/A"
            })
            .FirstOrDefaultAsync();
    }

    public async Task<GameResponseDto?> UpdateGameAsync(int id, CreateGameDto dto)
    {
        var game = await _dbContext.Games.FindAsync(id);
        if (game == null) return null;

        // Cek validasi Foreign Key jika diubah
        var publisherExists = await _dbContext.Publishers.AnyAsync(p => p.Id == dto.PublisherId);
        var genreExists = await _dbContext.Genres.AnyAsync(g => g.Id == dto.GenreId);
        if (!publisherExists || !genreExists) return null;

        // Update data
        game.Title = dto.Title;
        game.Price = dto.Price;
        game.Stock = dto.Stock;
        game.PublisherId = dto.PublisherId;
        game.GenreId = dto.GenreId;

        await _dbContext.SaveChangesAsync();

        // Mengembalikan data game terbaru (lengkap dengan PublisherName & GenreName)
        return await GetGameByIdAsync(game.Id);
    }

    public async Task<bool> DeleteGameAsync(int id)
    {
        var game = await _dbContext.Games.FindAsync(id);
        if (game == null) return false;

        _dbContext.Games.Remove(game);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}