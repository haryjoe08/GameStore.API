namespace GameStoreApi.DTOs;

public record TransactionResponseDto
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public string? GameTitle { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime TransactionDate { get; set; }
}