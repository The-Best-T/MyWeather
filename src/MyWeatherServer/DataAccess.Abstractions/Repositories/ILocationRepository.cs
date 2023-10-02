using Domain;

namespace DataAccess.Abstractions.Repositories;

public interface ILocationRepository
{
    Task<IEnumerable<Location>> GetLocationsByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<int> GetCountByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<Location?> GetLocationByIdAndUserIdAsync(
        Guid id,
        string userId,
        CancellationToken cancellation = default);

    Task<Location> CreateLocationAsync(
        string userId,
        Location newLocation,
        CancellationToken cancellationToken = default);

    Task<Location> UpdateLocationAsync(
        Location newLocation,
        CancellationToken cancellationToken = default);

    Task<Location> DeleteLocationAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}