using AutoMapper;
using Domain;
using UseCases.Commands.CreateLocation;
using UseCases.Commands.UpdateLocation;
using UseCases.Dto;

namespace UseCases.Mappers;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<Location, LocationDto>();
        CreateMap<CreateLocationCommand, Location>();
        CreateMap<UpdateLocationCommand, Location>();
    }
}