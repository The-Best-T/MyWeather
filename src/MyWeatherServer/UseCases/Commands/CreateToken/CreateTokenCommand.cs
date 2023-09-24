using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.CreateToken;

public record CreateTokenCommand(
    string Email,
    string Password) : IRequest<TokenDto>
{
}