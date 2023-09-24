using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UseCases.Abstractions;
using UseCases.Dto;

namespace UseCases.Commands.CreateToken;

internal class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, TokenDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IAuthenticationService _authenticationService;

    public CreateTokenCommandHandler(
        UserManager<IdentityUser> userManager,
        IAuthenticationService authenticationService)
    {
        _userManager = userManager;
        _authenticationService = authenticationService;
    }

    public async Task<TokenDto> Handle(
        CreateTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new ValidationException("Email or password is invalid");
        }

        var token = await _authenticationService.CreateTokenAsync(request.Email);

        return new TokenDto
        {
            Token = token,
        };
    }
}