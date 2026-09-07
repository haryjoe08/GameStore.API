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

    public async Task<ApiResponse<IEnumerable<TransactionResponseDto>>> GetAllTransactionsAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        var transactionDtos = _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);

        return ApiResponse<IEnumerable<TransactionResponseDto>>.Success(transactionDtos, "Transaction list retrieved successfully.");
    }

    public async Task<ApiResponse<TransactionResponseDto>> GetTransactionByIdAsync(int id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);
        if (transaction == null)
        {
            return ApiResponse<TransactionResponseDto>.Failure("Transaction not found.");
        }

        var transactionDto = _mapper.Map<TransactionResponseDto>(transaction);
        return ApiResponse<TransactionResponseDto>.Success(transactionDto, "Transaction details retrieved successfully.");
    }

    public async Task<ApiResponse<TransactionResponseDto>> CreateTransactionAsync(CreateTransactionDto dto)
    {
        // 1. Validasi Kuantitas
        if (dto.Quantity <= 0)
        {
            return ApiResponse<TransactionResponseDto>.Failure("Quantity must be greater than 0.");
        }

        // 2. Cek Keberadaan Game
        var game = await _gameRepository.GetByIdAsync(dto.GameId);
        if (game == null)
        {
            return ApiResponse<TransactionResponseDto>.Failure("Game not found.");
        }

        // 3. Validasi Ketersediaan Stok
        if (game.Stock < dto.Quantity)
        {
            return ApiResponse<TransactionResponseDto>.Failure($"Insufficient stock. Available stock: {game.Stock}.");
        }

        // 4. Kalkulasi Otomatis Total Price & Waktu Transaksi
        var transaction = _mapper.Map<Transaction>(dto);
        transaction.TotalPrice = game.Price * dto.Quantity;
        transaction.TransactionDate = DateTime.UtcNow;

        // 5. Potong Stok Game
        game.Stock -= dto.Quantity;
        await _gameRepository.UpdateAsync(game);

        // 6. Simpan Transaksi
        var createdTransaction = await _transactionRepository.CreateAsync(transaction);

        var transactionDto = _mapper.Map<TransactionResponseDto>(createdTransaction);
        return ApiResponse<TransactionResponseDto>.Success(transactionDto, "Transaction created successfully.");
    }
}