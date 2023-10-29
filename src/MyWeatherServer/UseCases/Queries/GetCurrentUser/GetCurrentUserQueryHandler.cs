using AutoMapper;
using Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UseCases.Dto;

namespace UseCases.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;

    public GetCurrentUserQueryHandler(
        UserManager<IdentityUser> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user == null)
        {
            throw new EntityNotFoundException("User does not exist");
        }

        return _mapper.Map<UserDto>(user);
    }
}