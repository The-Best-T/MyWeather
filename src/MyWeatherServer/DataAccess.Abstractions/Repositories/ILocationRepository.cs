using Domain;

namespace DataAccess.Abstractions.Repositories;

public interface ILocationRepository
{
    Task<IEnumerable<Location>> GetLocationsByUserIdAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}