using AutoMapper;
using GameStoreApi.DTOs;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using GameStoreApi.Services.Interfaces;

namespace GameStoreApi.Services;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;
    private readonly IGameRepository _gameRepository;
    private readonly IMapper _mapper;

    public PublisherService(
        IPublisherRepository publisherRepository,
        IGameRepository gameRepository,
        IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _gameRepository = gameRepository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<PublisherResponseDto>>> GetAllPublishersAsync()
    {
        var publishers = await _publisherRepository.GetAllAsync();
        var publisherDtos = _mapper.Map<IEnumerable<PublisherResponseDto>>(publishers);

        return ApiResponse<IEnumerable<PublisherResponseDto>>.Success(publisherDtos, "Publisher list retrieved successfully.");
    }

    public async Task<ApiResponse<PublisherResponseDto>> GetPublisherByIdAsync(int id)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id);
        if (publisher == null)
        {
            return ApiResponse<PublisherResponseDto>.Failure("Publisher not found.");
        }

        var publisherDto = _mapper.Map<PublisherResponseDto>(publisher);
        return ApiResponse<PublisherResponseDto>.Success(publisherDto, "Publisher details retrieved successfully.");
    }

    public async Task<ApiResponse<PublisherResponseDto>> CreatePublisherAsync(CreatePublisherDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return ApiResponse<PublisherResponseDto>.Failure("Publisher name is required.");
        }

        var nameExists = await _publisherRepository.IsNameExistsAsync(dto.Name);
        if (nameExists)
        {
            return ApiResponse<PublisherResponseDto>.Failure("Publisher with the same name already exists.");
        }

        var publisher = _mapper.Map<Publisher>(dto);
        var createdPublisher = await _publisherRepository.CreateAsync(publisher);

        var publisherDto = _mapper.Map<PublisherResponseDto>(createdPublisher);
        return ApiResponse<PublisherResponseDto>.Success(publisherDto, "Publisher created successfully.");
    }

    public async Task<ApiResponse<PublisherResponseDto>> UpdatePublisherAsync(int id, CreatePublisherDto dto)
    {
        var publisher = await _publisherRepository.GetByIdAsync(id);
        if (publisher == null)
        {
            return ApiResponse<PublisherResponseDto>.Failure("Publisher not found.");
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return ApiResponse<PublisherResponseDto>.Failure("Publisher name is required.");
        }

        var nameExists = await _publisherRepository.IsNameExistsAsync(dto.Name, excludeId: id);
        if (nameExists)
        {
            return ApiResponse<PublisherResponseDto>.Failure("Publisher with the same name already exists.");
        }

        _mapper.Map(dto, publisher);
        var updatedPublisher = await _publisherRepository.UpdateAsync(publisher);

        var publisherDto = _mapper.Map<PublisherResponseDto>(updatedPublisher);
        return ApiResponse<PublisherResponseDto>.Success(publisherDto, "Publisher updated successfully.");
    }

    public async Task<ApiResponse<bool>> DeletePublisherAsync(int id)
    {
        var exists = await _publisherRepository.ExistsAsync(id);
        if (!exists)
        {
            return ApiResponse<bool>.Failure("Publisher not found.");
        }

        // Integrity check: Mencegah penghapusan jika Publisher masih digunakan oleh Game
        var allGames = await _gameRepository.GetAllAsync();
        var isUsedInGames = allGames.Any(g => g.PublisherId == id);
        if (isUsedInGames)
        {
            return ApiResponse<bool>.Failure("Cannot delete publisher because it is currently assigned to one or more games.");
        }

        await _publisherRepository.DeleteAsync(id);
        return ApiResponse<bool>.Success(true, "Publisher deleted successfully.");
    }
}