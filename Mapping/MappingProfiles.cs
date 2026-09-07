using AutoMapper;
using GameStoreApi.DTOs;
using GameStoreApi.Models;

namespace GameStoreApi.Mapping;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Genre, GenreResponseDto>();
        CreateMap<CreateGenreDto, Genre>();
        
        CreateMap<Publisher, PublisherResponseDto>();
        CreateMap<CreatePublisherDto, Publisher>();
        
        CreateMap<CreateGameDto, Game>();
        // Custom Mapping dari Entity Game ke GameResponseDto
        // Mengambil Nama dari Navigation Property (Publisher.Name & Genre.Name)
        CreateMap<Game, GameResponseDto>()
            .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Publisher != null ? src.Publisher.Name : "N/A"))
            .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre != null ? src.Genre.Name : "N/A"));
        
        CreateMap<CreateTransactionDto, Transaction>();
        // Custom Mapping dari Entity Transaction ke TransactionResponseDto
        CreateMap<Transaction, TransactionResponseDto>()
            .ForMember(dest => dest.GameTitle, opt => opt.MapFrom(src => src.Game != null ? src.Game.Title : "N/A"));
    }
  
}