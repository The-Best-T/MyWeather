using AutoMapper;
using Controllers.Contracts.Output;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Queries.GetLocations;
using Utils.Extensions;

namespace Controllers.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LocationsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public LocationsController(
        IMapper mapper,
        ISender sender)
    {
        _mapper = mapper;
        _sender = sender;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationOutput>>> GetOwnLocationsAsync()
    {
        var getLocations = new GetLocationsQuery(HttpContext.User.GetUserId());
        var locations = await _sender.Send(getLocations, HttpContext.RequestAborted);

        return Ok(_mapper.Map<IEnumerable<LocationOutput>>(locations));
    }
}