using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.TripPlanner.Common;

namespace TourGuideBD.Application.Features.TripPlanner.Queries.CalculateRoute;

public class CalculateRouteQuery : IRequest<RouteDto>
{
    public double OriginLat { get; set; }
    public double OriginLng { get; set; }
    public double DestLat { get; set; }
    public double DestLng { get; set; }

    /// <summary>
    /// driving, driving-traffic, walking, cycling
    /// </summary>
    public string Profile { get; set; } = "driving";
}

public class CalculateRouteQueryValidator : AbstractValidator<CalculateRouteQuery>
{
    private static readonly string[] AllowedProfiles = { "driving", "driving-traffic", "walking", "cycling" };

    public CalculateRouteQueryValidator()
    {
        RuleFor(x => x.OriginLat).InclusiveBetween(-90, 90);
        RuleFor(x => x.OriginLng).InclusiveBetween(-180, 180);
        RuleFor(x => x.DestLat).InclusiveBetween(-90, 90);
        RuleFor(x => x.DestLng).InclusiveBetween(-180, 180);

        RuleFor(x => x.Profile)
            .Must(p => AllowedProfiles.Contains(p))
            .WithMessage($"Profile must be one of: {string.Join(", ", AllowedProfiles)}");
    }
}

public class CalculateRouteQueryHandler : IRequestHandler<CalculateRouteQuery, RouteDto>
{
    private readonly IMapboxService _mapboxService;

    public CalculateRouteQueryHandler(IMapboxService mapboxService)
    {
        _mapboxService = mapboxService;
    }

    public async Task<RouteDto> Handle(CalculateRouteQuery request, CancellationToken cancellationToken)
    {
        var result = await _mapboxService.GetRouteAsync(
            request.OriginLat, request.OriginLng,
            request.DestLat, request.DestLng,
            request.Profile, cancellationToken);

        return new RouteDto
        {
            DistanceKm = result.DistanceKm,
            DurationMinutes = result.DurationMinutes,
            Geometry = result.Geometry
        };
    }
}