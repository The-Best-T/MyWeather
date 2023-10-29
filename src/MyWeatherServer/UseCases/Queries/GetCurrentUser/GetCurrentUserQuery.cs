using MediatR;
using UseCases.Dto;

namespace UseCases.Queries.GetCurrentUser;

public record GetCurrentUserQuery(
    string UserId) : IRequest<UserDto>
{
}