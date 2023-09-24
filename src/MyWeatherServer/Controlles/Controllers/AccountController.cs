using AutoMapper;
using Controllers.Contracts.Input;
using Controllers.Contracts.Output;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Commands.CreateToken;
using UseCases.Commands.CreateUser;

namespace Controllers.Controllers;

[Route("api/[controller]")]
[ApiController]
internal class AccountController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public AccountController(
        ISender sender,
        IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<TokenOutput>> CreateUserAsync(
        [FromBody] CreateUserInput createUserInput)
    {
        var createdUser = await _sender.Send(new CreateUserCommand(
                                  createUserInput.Email,
                                  createUserInput.Password),
                              HttpContext.RequestAborted);

        var token = await _sender.Send(new CreateTokenCommand(
                            createdUser.Email, 
                            createdUser.Password),
                        HttpContext.RequestAborted);

        return Ok(_mapper.Map<TokenOutput>(token));
    }

    [HttpPost("token")]
    public async Task<ActionResult<TokenOutput>> CreateTokenAsync(
        [FromBody] CreateTokenInput createTokenInput)
    {
        var token = await _sender.Send(new CreateTokenCommand(
                            createTokenInput.Email, 
                            createTokenInput.Password),
                        HttpContext.RequestAborted);

        return Ok(_mapper.Map<TokenOutput>(token));
    }
}