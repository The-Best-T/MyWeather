using AutoMapper;
using Controllers.Contracts.Input;
using Controllers.Contracts.Output;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Commands.CreateToken;
using UseCases.Commands.CreateUser;
using UseCases.Queries.GetCurrentUser;
using Utils.Extensions;

namespace Controllers.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
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

    [HttpGet("current")]
    [Authorize]
    public async Task<ActionResult<UserOutput>> GetCurrentUser()
    {
        var currentUser = await _sender.Send(new GetCurrentUserQuery(
                                  HttpContext.User.GetUserId()), 
                              HttpContext.RequestAborted);

        return Ok(_mapper.Map<UserOutput>(currentUser));
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