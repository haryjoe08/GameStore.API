using System.ComponentModel.DataAnnotations;

namespace GameStoreApi.DTOs;

public record CreateGenreDto
{
    public string Name { get; set; } = string.Empty;
}