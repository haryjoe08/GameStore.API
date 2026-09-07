using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IGenreService
{
    Task<ServiceResult<IEnumerable<GenreResponseDto>>> GetAllGenresAsync();
    Task<ServiceResult<GenreResponseDto>> GetGenreByIdAsync(int id);
    Task<ServiceResult<GenreResponseDto>> CreateGenreAsync(CreateGenreDto dto);
    Task<ServiceResult<GenreResponseDto>> UpdateGenreAsync(int id, CreateGenreDto dto);
    Task<ServiceResult<bool>> DeleteGenreAsync(int id);
}