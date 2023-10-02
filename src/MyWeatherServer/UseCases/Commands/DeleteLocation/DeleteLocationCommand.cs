using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.DeleteLocation;

public record DeleteLocationCommand(
    Guid Id,
    string UserId) : IRequest<LocationDto>
{
}