using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionResponseDto?>> GetAllAsync();
    Task<TransactionResponseDto?> GetByIdAsync(int id);
    Task<TransactionResponseDto?> CreateAsync(CreateTransactionDto dto);
}