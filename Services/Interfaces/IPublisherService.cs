using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IPublisherService
{
    Task<IEnumerable<PublisherResponseDto>> GetAllAsync();
    Task<PublisherResponseDto?> GetByIdAsync(int id);
    Task<PublisherResponseDto?> CreateAsync(CreatePublisherDto dto);
    Task<PublisherResponseDto?> UpdateAsync(int id, CreatePublisherDto dto); // Return DTO (bisa null jika ID tidak ditemukan)
    Task<bool?> DeleteAsync(int id);
}