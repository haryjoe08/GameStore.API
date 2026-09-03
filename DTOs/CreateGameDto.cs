using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.DTOs;


public record CreateGameDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Required]
    public int PublisherId { get; set; }

    [Required]
    public int GenreId { get; init; }
}