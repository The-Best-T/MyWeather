using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UseCases.Dto;

namespace UseCases.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<IdentityUser, UserDto>();
    }
}