using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.DTOs;

public record CreatePublisherDto
{
    public string Name { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
}