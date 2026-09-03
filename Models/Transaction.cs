namespace GameStoreApi.Models;

public class Transaction
{
    public int Id { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    
    public int GameId { get; set; }
    public Game? Game { get; set; }
}