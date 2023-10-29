using AutoMapper;
using Controllers.Contracts.Output;
using UseCases.Dto;

namespace Controllers.Mappers;

public class TokenProfile : Profile
{
    public TokenProfile()
    {
        CreateMap<TokenDto, TokenOutput>();
    }
}