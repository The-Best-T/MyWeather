using AutoMapper;
using DataAccess.Abstractions.Repositories;
using Domain;
using Exceptions;
using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.UpdateLocation;

public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, LocationDto>
{
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;

    public UpdateLocationCommandHandler(
        ILocationRepository locationRepository,
        IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<LocationDto> Handle(
        UpdateLocationCommand request,
        CancellationToken cancellationToken)
    {
        var location = await _locationRepository.GetLocationByIdAndUserIdAsync(request.Id, request.UserId, cancellationToken);

        if (location is null)
        {
            throw new EntityNotFoundException("You don't have such a location");
        }

        var locationToUpdate = _mapper.Map<Location>(request);
        var updatedLocation = await _locationRepository.UpdateLocationAsync(locationToUpdate, cancellationToken);
        await _locationRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LocationDto>(updatedLocation);
    }
}