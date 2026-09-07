using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.DTOs;

public record CreateTransactionDto
{
    public int GameId { get; set; }

    public int Quantity { get; set; }
}