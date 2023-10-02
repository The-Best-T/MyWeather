using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.CreateLocation;

public record CreateLocationCommand(
    string Name,
    double Latitude,
    double Longitude,
    string UserId) : IRequest<LocationDto>
{
}