using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess.Abstractions.Repositories;
using Domain;
using Exceptions;
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
                     .OrderBy(x => x.Name)
                     .ProjectTo<Location>(_mapper.ConfigurationProvider)
                     .ToListAsync(cancellationToken);
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

    public async Task<Location?> GetLocationByIdAndUserIdAsync(
        Guid id,
        string userId,
        CancellationToken cancellation = default)
    {
        return await _weatherContext.Locations
                                    .Where(x => x.UserId == userId && x.Id == id)
                                    .ProjectTo<Location>(_mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync(cancellation);
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

    public async Task<Location> UpdateLocationAsync(
        Location newLocation,
        CancellationToken cancellationToken = default)
    {
        var entity = await _weatherContext.Locations
                                          .FirstOrDefaultAsync(x => x.Id == newLocation.Id, cancellationToken) ??
                     throw new EntityNotFoundException(nameof(Location));

        _mapper.Map(newLocation,entity);

        return _mapper.Map<Location>(entity);
    }

    public async Task<Location> DeleteLocationAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _weatherContext.Locations
                                          .FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ??
                     throw new EntityNotFoundException(nameof(Location));

        _weatherContext.Locations.Remove(entity);

        return _mapper.Map<Location>(entity);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _weatherContext.SaveChangesAsync(cancellationToken);
    }
}