namespace GameStoreApi.Models;

public class Game
{
    public int Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    
    public int PublisherId { get; set; }
    public Publisher? Publisher { get; set; }

    public int GenreId { get; set; }
    public Genre? Genre { get; set; }

    public List<Transaction> Transactions { get; set; } = new();
}