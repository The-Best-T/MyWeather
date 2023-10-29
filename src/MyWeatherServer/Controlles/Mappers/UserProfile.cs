using AutoMapper;
using Controllers.Contracts.Output;
using UseCases.Dto;

namespace Controllers.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, UserOutput>();
    }
}