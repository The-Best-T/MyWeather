using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess.Abstractions.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Npgsql.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly WeatherContext _weatherContext;
    private readonly IMapper _mapper;

    public LocationRepository(
        WeatherContext weatherContext,
        IMapper mapper)
    {
        _weatherContext = weatherContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Location>> GetLocationsByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _weatherContext
                     .Locations
                     .Where(x => x.UserId == userId)
                     .ProjectTo<Location>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _weatherContext.SaveChangesAsync(cancellationToken);
    }
}