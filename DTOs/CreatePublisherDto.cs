using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.DTOs;

public record CreatePublisherDto
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; init; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Country { get; init; } = string.Empty;
}