using MediatR;
using UseCases.Dto;

namespace UseCases.Queries.GetLocations;

public record GetLocationsQuery(
    string userId) : IRequest<IEnumerable<LocationDto>>
{
}