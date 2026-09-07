using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface ITransactionService
{
    Task<ApiResponse<IEnumerable<TransactionResponseDto>>> GetAllTransactionsAsync();
    Task<ApiResponse<TransactionResponseDto>> GetTransactionByIdAsync(int id);
    Task<ApiResponse<TransactionResponseDto>> CreateTransactionAsync(CreateTransactionDto dto);
}