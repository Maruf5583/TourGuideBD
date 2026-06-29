using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.TripPlanner.Common;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.TripPlanner.Queries.GetTripBudget;

public class GetTripBudgetQuery : IRequest<TripBudgetDto>
{
    public int PlaceId { get; set; }
    public int TransportTypeId { get; set; }

    public double OriginLat { get; set; }
    public double OriginLng { get; set; }

    /// <summary>
    /// Estimated food cost per person for the trip (BDT). Defaults to a standard day-trip estimate.
    /// </summary>
    public decimal? FoodCostOverride { get; set; }

    /// <summary>
    /// Number of people - food cost is multiplied by this.
    /// </summary>
    public int PeopleCount { get; set; } = 1;
}

public class GetTripBudgetQueryValidator : AbstractValidator<GetTripBudgetQuery>
{
    public GetTripBudgetQueryValidator()
    {
        RuleFor(x => x.PlaceId).GreaterThan(0);
        RuleFor(x => x.TransportTypeId).GreaterThan(0);
        RuleFor(x => x.OriginLat).InclusiveBetween(-90, 90);
        RuleFor(x => x.OriginLng).InclusiveBetween(-180, 180);
        RuleFor(x => x.PeopleCount).GreaterThan(0);
        RuleFor(x => x.FoodCostOverride).GreaterThanOrEqualTo(0).When(x => x.FoodCostOverride.HasValue);
    }
}

public class GetTripBudgetQueryHandler : IRequestHandler<GetTripBudgetQuery, TripBudgetDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapboxService _mapboxService;

    private const decimal DefaultFoodCostPerPerson = 300m; // BDT default day-trip food estimate

    public GetTripBudgetQueryHandler(IApplicationDbContext context, IMapboxService mapboxService)
    {
        _context = context;
        _mapboxService = mapboxService;
    }

    public async Task<TripBudgetDto> Handle(GetTripBudgetQuery request, CancellationToken cancellationToken)
    {
        var place = await _context.Places
            .FirstOrDefaultAsync(p => p.Id == request.PlaceId && p.ApprovalStatus == ApprovalStatus.Approved, cancellationToken);

        if (place == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Location.Place), request.PlaceId);
        }

        var rate = await _context.TransportRates
            .Include(r => r.TransportType)
            .FirstOrDefaultAsync(r => r.TransportTypeId == request.TransportTypeId && r.IsActive, cancellationToken);

        if (rate == null)
        {
            throw new NotFoundException("TransportRate", request.TransportTypeId);
        }

        var route = await _mapboxService.GetRouteAsync(
            request.OriginLat, request.OriginLng,
            place.Latitude, place.Longitude,
            "driving-traffic", cancellationToken);

        var rawCost = (decimal)route.DistanceKm * rate.RatePerKm;
        var transportCost = Math.Max(rawCost, rate.MinimumFare);

        var foodCostPerPerson = request.FoodCostOverride ?? DefaultFoodCostPerPerson;
        var totalFoodCost = foodCostPerPerson * request.PeopleCount;

        var totalCost = transportCost + place.EntryFee + totalFoodCost;

        return new TripBudgetDto
        {
            DistanceKm = route.DistanceKm,
            TransportCost = Math.Round(transportCost, 2),
            EntryFee = place.EntryFee,
            EstimatedFoodCost = Math.Round(totalFoodCost, 2),
            TotalCost = Math.Round(totalCost, 2),
            EstimatedTravelTimeMinutes = route.DurationMinutes,
            TransportTypeName = rate.TransportType.Name
        };
    }
}