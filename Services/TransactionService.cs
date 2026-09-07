using AutoMapper;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using GameStoreApi.Services.Interfaces;

namespace GameStoreApi.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IMapper _mapper;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IGameRepository gameRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _gameRepository = gameRepository;
        _mapper = mapper;
    }

    public async Task<ServiceResult<IEnumerable<TransactionResponseDto>>> GetAllTransactionsAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var transactionDtos = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);

        return ServiceResult<IEnumerable<TransactionResponseDto>>.Success(transactionDtos, "Transaction list retrieved successfully.");
    }

    public async Task<ServiceResult<TransactionResponseDto>> GetTransactionByIdAsync(int id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if (transaction == null)
        {
            return ServiceResult<TransactionResponseDto>.Failure("Transaction not found.");
        }

        var transactionDto = _mapper.Map<TransactionResponseDto>(transaction);
        return ServiceResult<TransactionResponseDto>.Success(transactionDto, "Transaction details retrieved successfully.");
    }

    public async Task<ServiceResult<TransactionResponseDto>> CreateTransactionAsync(CreateTransactionDto dto)
    {
        
        var game = await _gameRepository.GetByIdAsync(dto.GameId);
        if (game == null)
        {
            return ServiceResult<TransactionResponseDto>.Failure("Game not found.");
        }
        
        if (game.Stock < dto.Quantity)
        {
            return ServiceResult<TransactionResponseDto>.Failure($"Insufficient stock. Available stock: {game.Stock}.");
        }

        // Kalkulasi Otomatis Total Price & Waktu Transaksi
        var transaction = _mapper.Map<Transaction>(dto);
        transaction.TotalPrice = game.Price * dto.Quantity;
        transaction.TransactionDate = DateTime.UtcNow;

      
        game.Stock -= dto.Quantity;
        await _gameRepository.UpdateAsync(game);
        
        var createdTransaction = await _transactionRepository.CreateAsync(transaction);

        var transactionDto = _mapper.Map<TransactionResponseDto>(createdTransaction);
        return ServiceResult<TransactionResponseDto>.Success(transactionDto, "Transaction created successfully.");
    }
}