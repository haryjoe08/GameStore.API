using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.DTOs;


public record CreateGameDto
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int PublisherId { get; set; }
    public int GenreId { get; init; }
}