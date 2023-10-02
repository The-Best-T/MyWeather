using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DataAccess.Abstractions.Repositories;
using Domain;
using MediatR;
using UseCases.Dto;

namespace UseCases.Commands.CreateLocation;

public class CreateLocationCommandHandler : IRequestHandler<CreateLocationCommand, LocationDto>
{
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;
    public CreateLocationCommandHandler(
        ILocationRepository locationRepository,
        IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<LocationDto> Handle(
        CreateLocationCommand request,
        CancellationToken cancellationToken)
    {
        var count = await _locationRepository.GetCountByUserIdAsync(request.UserId, cancellationToken);

        if (count >= 30)
        {
            throw new ValidationException("You have the maximum number of locations.");
        }

        var locationToCreate = _mapper.Map<Location>(request);

        var createdLocation = await _locationRepository.CreateLocationAsync(request.UserId, locationToCreate, cancellationToken);
        await _locationRepository.SaveChangesAsync(cancellationToken);

        return _mapper.Map<LocationDto>(createdLocation);
    }
}