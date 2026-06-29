using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.TripPlanner.Queries.EstimateTravelTime;

public class TravelTimeResultDto
{
    public int TransportTypeId { get; set; }
    public string TransportTypeName { get; set; } = string.Empty;
    public double DistanceKm { get; set; }
    public double EstimatedTimeMinutes { get; set; }

    /// <summary>
    /// True if Mapbox traffic-aware duration was used (driving/driving-traffic profile);
    /// false if calculated from static average speed.
    /// </summary>
    public bool TrafficAware { get; set; }
}

public class EstimateTravelTimeQuery : IRequest<TravelTimeResultDto>
{
    public int TransportTypeId { get; set; }
    public double DistanceKm { get; set; }

    /// <summary>
    /// Optional - if provided with destination, Mapbox traffic-aware duration is used for Car/CNG/Bike (road-based modes).
    /// </summary>
    public double? OriginLat { get; set; }
    public double? OriginLng { get; set; }
    public double? DestLat { get; set; }
    public double? DestLng { get; set; }
}

public class EstimateTravelTimeQueryValidator : AbstractValidator<EstimateTravelTimeQuery>
{
    public EstimateTravelTimeQueryValidator()
    {
        RuleFor(x => x.TransportTypeId).GreaterThan(0);
        RuleFor(x => x.DistanceKm).GreaterThan(0);
    }
}

public class EstimateTravelTimeQueryHandler : IRequestHandler<EstimateTravelTimeQuery, TravelTimeResultDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapboxService _mapboxService;

    public EstimateTravelTimeQueryHandler(IApplicationDbContext context, IMapboxService mapboxService)
    {
        _context = context;
        _mapboxService = mapboxService;
    }

    public async Task<TravelTimeResultDto> Handle(EstimateTravelTimeQuery request, CancellationToken cancellationToken)
    {
        var rate = await _context.TransportRates
            .Include(r => r.TransportType)
            .FirstOrDefaultAsync(r => r.TransportTypeId == request.TransportTypeId && r.IsActive, cancellationToken);

        if (rate == null)
        {
            throw new NotFoundException("TransportRate", request.TransportTypeId);
        }

        var hasCoordinates = request.OriginLat.HasValue && request.OriginLng.HasValue
            && request.DestLat.HasValue && request.DestLng.HasValue;

        // Road-based modes that benefit from traffic-aware Mapbox duration
        var roadBasedTypes = new[]
        {
            Domain.Enums.TransportTypeEnum.Car,
            Domain.Enums.TransportTypeEnum.CNG,
            Domain.Enums.TransportTypeEnum.Bike,
            Domain.Enums.TransportTypeEnum.Bus
        };

        if (hasCoordinates && roadBasedTypes.Contains(rate.TransportType.Type))
        {
            try
            {
                var route = await _mapboxService.GetRouteAsync(
                    request.OriginLat!.Value, request.OriginLng!.Value,
                    request.DestLat!.Value, request.DestLng!.Value,
                    "driving-traffic", cancellationToken);

                return new TravelTimeResultDto
                {
                    TransportTypeId = rate.TransportTypeId,
                    TransportTypeName = rate.TransportType.Name,
                    DistanceKm = route.DistanceKm,
                    EstimatedTimeMinutes = route.DurationMinutes,
                    TrafficAware = true
                };
            }
            catch
            {
                // Fall back to static calculation if Mapbox call fails
            }
        }

        var staticTime = rate.AverageSpeedKmh > 0
            ? (request.DistanceKm / rate.AverageSpeedKmh) * 60.0
            : 0;

        return new TravelTimeResultDto
        {
            TransportTypeId = rate.TransportTypeId,
            TransportTypeName = rate.TransportType.Name,
            DistanceKm = request.DistanceKm,
            EstimatedTimeMinutes = Math.Round(staticTime, 2),
            TrafficAware = false
        };
    }
}