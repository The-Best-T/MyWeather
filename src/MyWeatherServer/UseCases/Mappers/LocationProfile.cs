using AutoMapper;
using DataAccess.Npgsql.DatabaseEntities;
using UseCases.Dto;

namespace UseCases.Mappers;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<Location, LocationDto>();
    }
}