using AutoMapper;
using Controllers.Contracts.Output;
using UseCases.Dto;

namespace Controllers.Mappers;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<LocationDto, LocationOutput>();
    }
}