using AutoMapper;
using Domain;

namespace DataAccess.Npgsql.Mappers;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<DatabaseEntities.Location, Location>();
    }
}