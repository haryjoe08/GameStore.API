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

    public async Task<ServiceResult<IEnumerable<GameResponseDto>>> GetAllGamesAsync()
    {
        var games = await _gameRepository.GetAllAsync();

        var gameDtos = _mapper.Map<IEnumerable<GameResponseDto>>(games);
        return ServiceResult<IEnumerable<GameResponseDto>>.Success(gameDtos, "Game list retrieved successfully.");
    }

    public async Task<ServiceResult<GameResponseDto>> GetGameByIdAsync(int id)
    {
        var game = await _gameRepository.GetByIdAsync(id);

        if (game == null)
        {
            return ServiceResult<GameResponseDto>.Failure("Game not found.");
        }

        var gameDto = _mapper.Map<GameResponseDto>(game);
        return ServiceResult<GameResponseDto>.Success(gameDto, "Game details retrieved successfully.");
    }

    public async Task<ServiceResult<GameResponseDto>> CreateGameAsync(CreateGameDto dto)
    {
        var publisherExists = await _publisherRepository.ExistsAsync(dto.PublisherId);
        var genreExists = await _genreRepository.ExistsAsync(dto.GenreId);

        if (!publisherExists || !genreExists)
        {
            return ServiceResult<GameResponseDto>.Failure("Invalid PublisherId or GenreId.");
        }

        var game = _mapper.Map<Game>(dto);
        var createdGame = await _gameRepository.CreateAsync(game);

        var gameDto = _mapper.Map<GameResponseDto>(createdGame);
        return ServiceResult<GameResponseDto>.Success(gameDto, "Game created successfully.");
    }

    public async Task<ServiceResult<GameResponseDto>> UpdateGameAsync(int id, CreateGameDto dto)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null)
        {
            return ServiceResult<GameResponseDto>.Failure("Game not found.");
        }
        
        var publisherExists = await _publisherRepository.ExistsAsync(dto.PublisherId);
        var genreExists = await _genreRepository.ExistsAsync(dto.GenreId);
        if (!publisherExists || !genreExists)
        {
            return ServiceResult<GameResponseDto>.Failure("Publisher or Genre not found.");
        }

        _mapper.Map(dto, game);

        var updatedGame = await _gameRepository.UpdateAsync(game);

        var gameDto = _mapper.Map<GameResponseDto>(updatedGame);
        return ServiceResult<GameResponseDto>.Success(gameDto, "Game updated successfully.");
    }

    public async Task<ServiceResult<bool>> DeleteGameAsync(int id)
    {
        var exists = await _gameRepository.ExistsAsync(id);
        if (!exists)
        {
            return ServiceResult<bool>.Failure("Game not found.");
        }

        var allTransactions = await _transactionRepository.GetAllAsync();
        var hasTransactions = allTransactions.Any(t => t.GameId == id);

        if (hasTransactions)
        {
            return ServiceResult<bool>.Failure("Cannot delete game because it has associated transaction history.");
        }

        await _gameRepository.DeleteAsync(id);

        return ServiceResult<bool>.Success(true, "Game deleted successfully.");
    }
}