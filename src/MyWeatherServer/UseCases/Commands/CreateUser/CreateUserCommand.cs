using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.CreateUser;

public record CreateUserCommand(
    string Email,
    string Password) : IRequest<UserDto>
{
}