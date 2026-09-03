using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.DTOs;

public record CreateTransactionDto
{
    [Required]
    public int GameId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Jumlah pembelian minimal 1.")]
    public int Quantity { get; set; }
}
