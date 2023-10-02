using AutoMapper;
using DataAccess.Abstractions.Repositories;
using Exceptions;
using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.DeleteLocation;

public class DeleteLocationCommandHandler : IRequestHandler<DeleteLocationCommand, LocationDto>
{
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;

    public DeleteLocationCommandHandler(
        ILocationRepository locationRepository,
        IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<LocationDto> Handle(
        DeleteLocationCommand request,
        CancellationToken cancellationToken)
    {
        var location = await _locationRepository.GetLocationByIdAndUserIdAsync(request.Id, request.UserId, cancellationToken);

        if (location is null)
        {
            throw new EntityNotFoundException("You don't have such a location");
        }

        var deletedLocation = await _locationRepository.DeleteLocationAsync(location.Id, cancellationToken);
        await _locationRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LocationDto>(deletedLocation);
    }
}