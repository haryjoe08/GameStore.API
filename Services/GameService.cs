using AutoMapper;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using GameStoreApi.Services.Interfaces;

namespace GameStoreApi.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public GameService(
        IGameRepository gameRepository,
        IMapper mapper, IGenreRepository genreRepository, IPublisherRepository publisherRepository,
        ITransactionRepository transactionRepository)
    {
        _gameRepository = gameRepository;
        _publisherRepository = publisherRepository;
        _genreRepository = genreRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<GameResponseDto>>> GetAllGamesAsync()
    {
        var games = await _gameRepository.GetAllAsync();

        var gameDtos = _mapper.Map<IEnumerable<GameResponseDto>>(games);
        return ApiResponse<IEnumerable<GameResponseDto>>.Success(gameDtos, "Game list retrieved successfully.");
    }

    public async Task<ApiResponse<GameResponseDto>> GetGameByIdAsync(int id)
    {
        var game = await _gameRepository.GetByIdAsync(id);

        if (game == null)
        {
            return ApiResponse<GameResponseDto>.Failure("Game not found.");
        }

        var gameDto = _mapper.Map<GameResponseDto>(game);
        return ApiResponse<GameResponseDto>.Success(gameDto, "Game details retrieved successfully.");
    }

    public async Task<ApiResponse<GameResponseDto>> CreateGameAsync(CreateGameDto dto)
    {
        if (dto.Price <= 0)
        {
            return ApiResponse<GameResponseDto>.Failure("Price must be greater than 0.");
        }

        if (dto.Stock < 0)
        {
            return ApiResponse<GameResponseDto>.Failure("Stock cannot be negative.");
        }

        var publisherExists = await _publisherRepository.ExistsAsync(dto.PublisherId);
        var genreExists = await _genreRepository.ExistsAsync(dto.GenreId);

        if (!publisherExists || !genreExists)
        {
            return ApiResponse<GameResponseDto>.Failure("Invalid PublisherId or GenreId.");
        }

        var game = _mapper.Map<Game>(dto);
        var createdGame = await _gameRepository.CreateAsync(game);

        var gameDto = _mapper.Map<GameResponseDto>(createdGame);
        return ApiResponse<GameResponseDto>.Success(gameDto, "Game created successfully.");
    }

    public async Task<ApiResponse<GameResponseDto>> UpdateGameAsync(int id, CreateGameDto dto)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null)
        {
            return ApiResponse<GameResponseDto>.Failure("Game not found.");
        }

        if (dto.Price <= 0 || dto.Stock < 0)
        {
            return ApiResponse<GameResponseDto>.Failure("Price must be greater than 0 and stock cannot be negative.");
        }

        var publisherExists = await _publisherRepository.ExistsAsync(dto.PublisherId);
        var genreExists = await _genreRepository.ExistsAsync(dto.GenreId);
        if (!publisherExists || !genreExists)
        {
            return ApiResponse<GameResponseDto>.Failure("Publisher or Genre not found.");
        }

        _mapper.Map(dto, game);

        var updatedGame = await _gameRepository.UpdateAsync(game);

        var gameDto = _mapper.Map<GameResponseDto>(updatedGame);
        return ApiResponse<GameResponseDto>.Success(gameDto, "Game updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeleteGameAsync(int id)
    {
        var exists = await _gameRepository.ExistsAsync(id);
        if (!exists)
        {
            return ApiResponse<bool>.Failure("Game not found.");
        }

        var allTransactions = await _transactionRepository.GetAllAsync();
        var hasTransactions = allTransactions.Any(t => t.GameId == id);

        if (hasTransactions)
        {
            return ApiResponse<bool>.Failure("Cannot delete game because it has associated transaction history.");
        }

        await _gameRepository.DeleteAsync(id);

        return ApiResponse<bool>.Success(true, "Game deleted successfully.");
    }
}