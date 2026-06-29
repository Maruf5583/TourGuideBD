using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.ValueObjects;

namespace TourGuideBD.Application.Features.Places.Queries.GetNearbyPlaces;

public class GetNearbyPlacesQuery : IRequest<List<PlaceListItemDto>>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    /// <summary>
    /// Radius in KM - allowed values: 5, 10, 20
    /// </summary>
    public int RadiusKm { get; set; } = 5;
}

public class GetNearbyPlacesQueryValidator : AbstractValidator<GetNearbyPlacesQuery>
{
    private static readonly int[] AllowedRadii = { 5, 10, 20 };

    public GetNearbyPlacesQueryValidator()
    {
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        RuleFor(x => x.RadiusKm)
            .Must(r => AllowedRadii.Contains(r))
            .WithMessage("RadiusKm must be one of: 5, 10, 20");
    }
}

public class GetNearbyPlacesQueryHandler : IRequestHandler<GetNearbyPlacesQuery, List<PlaceListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNearbyPlacesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PlaceListItemDto>> Handle(GetNearbyPlacesQuery request, CancellationToken cancellationToken)
    {
        // Bounding box pre-filter to reduce rows before in-memory Haversine calc
        const double kmPerDegreeLat = 111.0;
        var latDelta = request.RadiusKm / kmPerDegreeLat;
        var lonDelta = request.RadiusKm / (kmPerDegreeLat * Math.Cos(ToRadians(request.Latitude)));

        var minLat = request.Latitude - latDelta;
        var maxLat = request.Latitude + latDelta;
        var minLon = request.Longitude - lonDelta;
        var maxLon = request.Longitude + lonDelta;

        var candidates = await _context.Places
            .Where(p => p.ApprovalStatus == ApprovalStatus.Approved
                && p.Latitude >= minLat && p.Latitude <= maxLat
                && p.Longitude >= minLon && p.Longitude <= maxLon)
            .Include(p => p.Photos)
            .Include(p => p.CategoryMaps).ThenInclude(cm => cm.PlaceCategory)
            .Include(p => p.District)
            .Include(p => p.Division)
            .ToListAsync(cancellationToken);

        var origin = new GeoCoordinate(request.Latitude, request.Longitude);

        var result = candidates
            .Select(p =>
            {
                var dto = _mapper.Map<PlaceListItemDto>(p);
                dto.DistanceKm = Math.Round(origin.DistanceToInKm(p.Coordinate), 2);
                return dto;
            })
            .Where(d => d.DistanceKm <= request.RadiusKm)
            .OrderBy(d => d.DistanceKm)
            .ToList();

        return result;
    }

    private static double ToRadians(double degrees) => degrees * (Math.PI / 180);
}