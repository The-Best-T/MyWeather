using AutoMapper;
using DataAccess.Npgsql.DatabaseEntities;
using UseCases.Commands.CreateLocation;
using UseCases.Dto;

namespace UseCases.Mappers;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<Location, LocationDto>();
        CreateMap<CreateLocationCommand, Location>();
    }
}