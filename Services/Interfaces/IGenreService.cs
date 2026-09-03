using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreResponseDto>> GetAllAsync();
    Task<GenreResponseDto?> GetByIdAsync(int id);
    Task<GenreResponseDto?> CreateAsync(CreateGenreDto dto);
    Task<GenreResponseDto?> UpdateAsync(int id, CreateGenreDto dto); // Return DTO (bisa null jika ID tidak ditemukan)
    Task<bool?> DeleteAsync(int id);
}