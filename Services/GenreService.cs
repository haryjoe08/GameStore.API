using AutoMapper;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using GameStoreApi.Services.Interfaces;

namespace GameStoreApi.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IMapper _mapper;

    public GenreService(
        IGenreRepository genreRepository,
        IGameRepository gameRepository,
        IMapper mapper)
    {
        _genreRepository = genreRepository;
        _gameRepository = gameRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<GenreResponseDto>>> GetAllGenresAsync()
    {
        var genres = await _genreRepository.GetAllAsync();
        var genreDtos = _mapper.Map<IEnumerable<GenreResponseDto>>(genres);

        return ApiResponse<IEnumerable<GenreResponseDto>>.Success(genreDtos, "Genre list retrieved successfully.");
    }

    public async Task<ApiResponse<GenreResponseDto>> GetGenreByIdAsync(int id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre == null)
        {
            return ApiResponse<GenreResponseDto>.Failure("Genre not found.");
        }

        var genreDto = _mapper.Map<GenreResponseDto>(genre);
        return ApiResponse<GenreResponseDto>.Success(genreDto, "Genre details retrieved successfully.");
    }

    public async Task<ApiResponse<GenreResponseDto>> CreateGenreAsync(CreateGenreDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return ApiResponse<GenreResponseDto>.Failure("Genre name is required.");
        }

        var nameExists = await _genreRepository.IsNameExistsAsync(dto.Name);
        if (nameExists)
        {
            return ApiResponse<GenreResponseDto>.Failure("Genre with the same name already exists.");
        }

        var genre = _mapper.Map<Genre>(dto);
        var createdGenre = await _genreRepository.CreateAsync(genre);

        var genreDto = _mapper.Map<GenreResponseDto>(createdGenre);
        return ApiResponse<GenreResponseDto>.Success(genreDto, "Genre created successfully.");
    }

    public async Task<ApiResponse<GenreResponseDto>> UpdateGenreAsync(int id, CreateGenreDto dto)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        if (genre == null)
        {
            return ApiResponse<GenreResponseDto>.Failure("Genre not found.");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return ApiResponse<GenreResponseDto>.Failure("Genre name is required.");
        }

        var nameExists = await _genreRepository.IsNameExistsAsync(dto.Name, excludeId: id);
        if (nameExists)
        {
            return ApiResponse<GenreResponseDto>.Failure("Genre with the same name already exists.");
        }

        _mapper.Map(dto, genre);
        var updatedGenre = await _genreRepository.UpdateAsync(genre);

        var genreDto = _mapper.Map<GenreResponseDto>(updatedGenre);
        return ApiResponse<GenreResponseDto>.Success(genreDto, "Genre updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteGenreAsync(int id)
    {
        var exists = await _genreRepository.ExistsAsync(id);
        if (!exists)
        {
            return ApiResponse<bool>.Failure("Genre not found.");
        }

        // Integrity check: Mencegah penghapusan jika Genre masih digunakan oleh Game
        var allGames = await _gameRepository.GetAllAsync();
        var isUsedInGames = allGames.Any(g => g.GenreId == id);
        if (isUsedInGames)
        {
            return ApiResponse<bool>.Failure("Cannot delete genre because it is currently assigned to one or more games.");
        }

        await _genreRepository.DeleteAsync(id);
        return ApiResponse<bool>.Success(true, "Genre deleted successfully.");
    }
}