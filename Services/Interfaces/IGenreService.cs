using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IGenreService
{
    Task<ApiResponse<IEnumerable<GenreResponseDto>>> GetAllGenresAsync();
    Task<ApiResponse<GenreResponseDto>> GetGenreByIdAsync(int id);
    Task<ApiResponse<GenreResponseDto>> CreateGenreAsync(CreateGenreDto dto);
    Task<ApiResponse<GenreResponseDto>> UpdateGenreAsync(int id, CreateGenreDto dto);
    Task<ApiResponse<bool>> DeleteGenreAsync(int id);
}