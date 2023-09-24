using AutoMapper;
using Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UseCases.Dto;

namespace UseCases.Commands.CreateUser;

internal class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public CreateUserCommandHandler(
        IMapper mapper,
        UserManager<IdentityUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<UserDto> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = _userManager.FindByEmailAsync(request.Email);

        if (user is not null)
        {
            throw new EntityConflictException($"User with email {request.Email} already exist");
        }

        var newUser = _mapper.Map<IdentityUser>(request);

        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
        {
            throw new AppException("Something went wrong");
        }

        return _mapper.Map<UserDto>(newUser);
    }
}