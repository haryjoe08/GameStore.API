namespace GameStoreApi.DTOs;


public record GameResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string PublisherName { get; set; } = string.Empty;
    public string GenreName { get; set; } = string.Empty;
}