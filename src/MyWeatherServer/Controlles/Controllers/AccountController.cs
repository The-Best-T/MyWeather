using Controllers.Contracts.Input;
using Controllers.Contracts.Output;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCases.Commands.CreateUser;

namespace Controllers.Controllers;

[Route("api/[controller]")]
[ApiController]
internal class AccountController : ControllerBase
{
    private readonly ISender _sender;

    public AccountController(
        ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("register")]
    public async Task<ActionResult<TokenOutput>> CreateUserAsync([FromBody]CreateUserInput createUserInput)
    {
        var createdUser =  await _sender.Send(new CreateUserCommand(
                createUserInput.Email,
                createUserInput.Password),
            HttpContext.RequestAborted);
        var token = await _sender.Send(new CreateTokenCommand(createdUser.Email), HttpContext.RequestAborted);

        return Ok(new TokenOutput
        {
            Token = token,
        });
    }
}