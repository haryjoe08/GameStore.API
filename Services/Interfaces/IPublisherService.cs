using GameStoreApi.DTOs;

namespace GameStoreApi.Services.Interfaces;

public interface IPublisherService
{
    Task<ServiceResult<IEnumerable<PublisherResponseDto>>> GetAllPublishersAsync();
    Task<ServiceResult<PublisherResponseDto>> GetPublisherByIdAsync(int id);
    Task<ServiceResult<PublisherResponseDto>> CreatePublisherAsync(CreatePublisherDto dto);
    Task<ServiceResult<PublisherResponseDto>> UpdatePublisherAsync(int id, CreatePublisherDto dto);
    Task<ServiceResult<bool>> DeletePublisherAsync(int id);
}