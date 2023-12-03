using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;

namespace Chinook.API.Profiles;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<Album, AlbumApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
        CreateMap<Artist, ArtistApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
        CreateMap<Customer, CustomerApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
        CreateMap<Employee, EmployeeApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
        CreateMap<Genre, GenreApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
        CreateMap<InvoiceLine, InvoiceLineApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
        CreateMap<Invoice, InvoiceApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
        CreateMap<MediaType, MediaTypeApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
        CreateMap<Playlist, PlaylistApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
        CreateMap<Track, TrackApiModel>()
            .EqualityComparison((api, e) => api.Id == e.Id)
            .MaxDepth(2).ReverseMap();
    }
}