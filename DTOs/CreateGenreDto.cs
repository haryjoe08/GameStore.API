using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.DTOs;

public record CreateGenreDto
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
}