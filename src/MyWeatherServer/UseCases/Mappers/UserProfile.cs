using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UseCases.Commands.CreateUser;
using UseCases.Dto;

namespace UseCases.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserCommand, UserDto>();
        CreateMap<CreateUserCommand, IdentityUser>()
            .ForMember(x => x.UserName, opts => opts.MapFrom(y => y.Email));
    }
}