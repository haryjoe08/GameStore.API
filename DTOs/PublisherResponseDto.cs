namespace GameStoreApi.DTOs;

public record PublisherResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}