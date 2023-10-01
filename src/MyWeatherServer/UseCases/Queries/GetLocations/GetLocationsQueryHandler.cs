using AutoMapper;
using DataAccess.Abstractions.Repositories;
using MediatR;
using UseCases.Dto;

namespace UseCases.Queries.GetLocations;

public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, IEnumerable<LocationDto>>
{
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;

    public GetLocationsQueryHandler(
        IMapper mapper,
        ILocationRepository locationRepository)
    {
        _mapper = mapper;
        _locationRepository = locationRepository;
    }

    public async Task<IEnumerable<LocationDto>> Handle(
        GetLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var locations = await _locationRepository.GetLocationsByUserIdAsync(request.userId, cancellationToken);

        return _mapper.Map<IEnumerable<LocationDto>>(locations);
    }
}