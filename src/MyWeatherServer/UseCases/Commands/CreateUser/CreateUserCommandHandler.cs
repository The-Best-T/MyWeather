using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.CreateUser;

internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    public CreateUserCommandHandler()
    {
        
    }

    public Task<UserDto> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
    }
}