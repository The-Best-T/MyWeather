using AutoMapper;
using Domain;

namespace DataAccess.Npgsql.Mappers;

public class LocationProfile : Profile
{
    public LocationProfile()
    {
        CreateMap<DatabaseEntities.Location, Location>();
        CreateMap<Location, DatabaseEntities.Location>()
            .ForMember(x => x.Id, opts => opts.Ignore())
            .ForMember(x => x.UserId, opts => opts.Ignore());
    }
}