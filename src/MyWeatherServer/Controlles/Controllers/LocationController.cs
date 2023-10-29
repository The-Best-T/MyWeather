using AutoMapper;
using Controllers.Contracts.Input;
using Controllers.Contracts.Output;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCases.Commands.CreateLocation;
using UseCases.Commands.DeleteLocation;
using UseCases.Commands.UpdateLocation;
using UseCases.Queries.GetLocations;
using Utils.Extensions;

namespace Controllers.Controllers;

[Route("api/locations")]
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

    [HttpPost]
    public async Task<ActionResult<LocationOutput>> CreateLocationAsync([FromBody] CreateLocationInput createLocationInput)
    {
        var createLocation = new CreateLocationCommand(
            createLocationInput.Name,
            createLocationInput.Latitude,
            createLocationInput.Longitude,
            HttpContext.User.GetUserId());
        var createdLocation = await _sender.Send(createLocation, HttpContext.RequestAborted);

        return Ok(_mapper.Map<LocationOutput>(createdLocation));
    }

    [HttpPut("{id:guid}")]

    public async Task<ActionResult<LocationOutput>> UpdateLocationAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateLocationInput updateLocationInput)
    {
        var updateLocation = new UpdateLocationCommand(
            id,
            updateLocationInput.Name,
            updateLocationInput.Latitude,
            updateLocationInput.Longitude,
            HttpContext.User.GetUserId());
        var updatedLocation = await _sender.Send(updateLocation, HttpContext.RequestAborted);

        return Ok(_mapper.Map<LocationOutput>(updatedLocation));
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<LocationOutput>> DeleteLocationAsync(
        [FromRoute] Guid id)
    {
        var deleteLocation = new DeleteLocationCommand(id, HttpContext.User.GetUserId());
        var deletedLocation = await _sender.Send(deleteLocation, HttpContext.RequestAborted);

        return Ok(_mapper.Map<LocationOutput>(deletedLocation));
    }
}