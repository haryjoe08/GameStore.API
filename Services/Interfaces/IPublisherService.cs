using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IPublisherService
{
    Task<ApiResponse<IEnumerable<PublisherResponseDto>>> GetAllPublishersAsync();
    Task<ApiResponse<PublisherResponseDto>> GetPublisherByIdAsync(int id);
    Task<ApiResponse<PublisherResponseDto>> CreatePublisherAsync(CreatePublisherDto dto);
    Task<ApiResponse<PublisherResponseDto>> UpdatePublisherAsync(int id, CreatePublisherDto dto);
    Task<ApiResponse<bool>> DeletePublisherAsync(int id);
}