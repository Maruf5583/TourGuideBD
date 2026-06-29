using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.TripPlanner.Common;
using TourGuideBD.Domain.Entities.Trip;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.TripPlanner.Commands.CreateItinerary;

public class CreateItineraryStopInput
{
    public int PlaceId { get; set; }
    public int Order { get; set; }
    public int TransportTypeId { get; set; }
}

public class CreateItineraryCommand : IRequest<ItineraryDto>, IAuditableRequest
{
    public string Title { get; set; } = string.Empty;
    public DateTime TripDate { get; set; }

    /// <summary>
    /// Starting point of the trip (e.g., user's home location)
    /// </summary>
    public double StartLat { get; set; }
    public double StartLng { get; set; }

    public List<CreateItineraryStopInput> Stops { get; set; } = new();

    public decimal FoodCostPerPersonPerStop { get; set; } = 300m;
    public int PeopleCount { get; set; } = 1;

    /// <summary>
    /// Set internally from ICurrentUserService
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    public string ActionName => "CreateItinerary";
    public string EntityName => nameof(Itinerary);
    public string? EntityId { get; set; }
}

public class CreateItineraryCommandValidator : AbstractValidator<CreateItineraryCommand>
{
    public CreateItineraryCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.TripDate).GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Trip date cannot be in the past.");

        RuleFor(x => x.StartLat).InclusiveBetween(-90, 90);
        RuleFor(x => x.StartLng).InclusiveBetween(-180, 180);

        RuleFor(x => x.PeopleCount).GreaterThan(0);
        RuleFor(x => x.FoodCostPerPersonPerStop).GreaterThanOrEqualTo(0);

        RuleFor(x => x.Stops)
            .NotEmpty().WithMessage("At least one stop is required.")
            .Must(stops => stops.Select(s => s.Order).Distinct().Count() == stops.Count)
            .WithMessage("Stop order values must be unique.");

        RuleForEach(x => x.Stops).ChildRules(stop =>
        {
            stop.RuleFor(s => s.PlaceId).GreaterThan(0);
            stop.RuleFor(s => s.TransportTypeId).GreaterThan(0);
            stop.RuleFor(s => s.Order).GreaterThanOrEqualTo(0);
        });
    }
}

public class CreateItineraryCommandHandler : IRequestHandler<CreateItineraryCommand, ItineraryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapboxService _mapboxService;

    public CreateItineraryCommandHandler(IApplicationDbContext context, IMapboxService mapboxService)
    {
        _context = context;
        _mapboxService = mapboxService;
    }

    public async Task<ItineraryDto> Handle(CreateItineraryCommand request, CancellationToken cancellationToken)
    {
        var orderedStops = request.Stops.OrderBy(s => s.Order).ToList();
        var placeIds = orderedStops.Select(s => s.PlaceId).Distinct().ToList();

        var places = await _context.Places
            .Where(p => placeIds.Contains(p.Id) && p.ApprovalStatus == ApprovalStatus.Approved)
            .ToDictionaryAsync(p => p.Id, cancellationToken);

        if (places.Count != placeIds.Count)
        {
            var missing = placeIds.Except(places.Keys);
            throw new NotFoundException("One or more places not found or not approved.", string.Join(",", missing));
        }

        var transportTypeIds = orderedStops.Select(s => s.TransportTypeId).Distinct().ToList();
        var rates = await _context.TransportRates
            .Include(r => r.TransportType)
            .Where(r => transportTypeIds.Contains(r.TransportTypeId) && r.IsActive)
            .ToDictionaryAsync(r => r.TransportTypeId, cancellationToken);

        if (rates.Count != transportTypeIds.Count)
        {
            var missing = transportTypeIds.Except(rates.Keys);
            throw new NotFoundException("One or more transport types not found or inactive.", string.Join(",", missing));
        }

        var itinerary = new Itinerary
        {
            UserId = request.UserId,
            Title = request.Title,
            TripDate = request.TripDate,
            CreatedAt = DateTime.UtcNow
        };

        decimal cumulativeTransportCost = 0;
        decimal cumulativeEntryFee = 0;

        double prevLat = request.StartLat;
        double prevLng = request.StartLng;

        foreach (var stopInput in orderedStops)
        {
            var place = places[stopInput.PlaceId];
            var rate = rates[stopInput.TransportTypeId];

            var route = await _mapboxService.GetRouteAsync(
                prevLat, prevLng, place.Latitude, place.Longitude, "driving-traffic", cancellationToken);

            var rawCost = (decimal)route.DistanceKm * rate.RatePerKm;
            var transportCost = Math.Max(rawCost, rate.MinimumFare);

            cumulativeTransportCost += transportCost;
            cumulativeEntryFee += place.EntryFee;

            itinerary.Stops.Add(new ItineraryStop
            {
                PlaceId = place.Id,
                Order = stopInput.Order,
                TransportTypeId = stopInput.TransportTypeId,
                DistanceFromPreviousKm = route.DistanceKm,
                TransportCost = Math.Round(transportCost, 2),
                TravelTimeMinutes = route.DurationMinutes,
                EntryFeeAtThisStop = place.EntryFee
            });

            prevLat = place.Latitude;
            prevLng = place.Longitude;
        }

        var totalFoodCost = request.FoodCostPerPersonPerStop * request.PeopleCount * orderedStops.Count;

        itinerary.EstimatedFoodCost = Math.Round(totalFoodCost, 2);
        itinerary.EstimatedTotalCost = Math.Round(cumulativeTransportCost + cumulativeEntryFee + totalFoodCost, 2);

        _context.Itineraries.Add(itinerary);
        await _context.SaveChangesAsync(cancellationToken);

        request.EntityId = itinerary.Id.ToString();

        return new ItineraryDto
        {
            Id = itinerary.Id,
            Title = itinerary.Title,
            TripDate = itinerary.TripDate,
            EstimatedTotalCost = itinerary.EstimatedTotalCost,
            EstimatedFoodCost = itinerary.EstimatedFoodCost,
            Stops = itinerary.Stops.Select(s => new ItineraryStopDto
            {
                PlaceId = s.PlaceId,
                PlaceName = places[s.PlaceId].Name,
                Order = s.Order,
                TransportTypeId = s.TransportTypeId,
                TransportTypeName = rates[s.TransportTypeId].TransportType.Name,
                DistanceFromPreviousKm = s.DistanceFromPreviousKm,
                TransportCost = s.TransportCost,
                TravelTimeMinutes = s.TravelTimeMinutes,
                EntryFeeAtThisStop = s.EntryFeeAtThisStop
            }).OrderBy(s => s.Order).ToList()
        };
    }
}