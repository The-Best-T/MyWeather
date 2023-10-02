using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.UpdateLocation;

public record UpdateLocationCommand(
    Guid Id,
    string Name,
    double Latitude,
    double Longitude,
    string UserId) : IRequest<LocationDto>
{
}