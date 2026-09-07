using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface ITransactionService
{
    Task<ServiceResult<IEnumerable<TransactionResponseDto>>> GetAllTransactionsAsync();
    Task<ServiceResult<TransactionResponseDto>> GetTransactionByIdAsync(int id);
    Task<ServiceResult<TransactionResponseDto>> CreateTransactionAsync(CreateTransactionDto dto);
}