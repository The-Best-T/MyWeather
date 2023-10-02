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

    public async Task<int> GetCountByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        return await _weatherContext
                     .Locations
                     .Where(x => x.UserId == userId)
                     .CountAsync(cancellationToken);
    }

    public async Task<Location> CreateLocationAsync(
        string userId,
        Location newLocation,
        CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<DatabaseEntities.Location>(newLocation);
        entity.UserId = userId;
        await _weatherContext.Locations.AddAsync(entity, cancellationToken);

        return _mapper.Map<Location>(entity);
    }
}