using Domain;

namespace DataAccess.Abstractions.Repositories;

public interface ILocationRepository
{
    Task<IEnumerable<Location>> GetLocationsByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<int> GetCountByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Location> CreateLocationAsync(
        string userId,
        Location newLocation,
        CancellationToken cancellationToken = default);
}