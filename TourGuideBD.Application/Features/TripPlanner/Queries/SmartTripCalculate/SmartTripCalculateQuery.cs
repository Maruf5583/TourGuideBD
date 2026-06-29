using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Trip;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using TourGuideBD.Domain.ValueObjects;

namespace TourGuideBD.Application.Features.TripPlanner.Queries.SmartTripCalculate;

// -------------------- DTOs --------------------

public class TripStepDto
{
    public int StepNumber { get; set; }

    // কী করতে হবে
    public string Instruction { get; set; } = string.Empty;
    public string InstructionBn { get; set; } = string.Empty;

    // Transport type
    public string TransportMode { get; set; } = string.Empty;
    public string TransportModeBn { get; set; } = string.Empty;

    // From → To
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;

    // Numbers
    public double? DistanceKm { get; set; }
    public decimal? Cost { get; set; }
    public double? TimeMinutes { get; set; }

    // Google Maps link for this step
    public string? GoogleMapsLink { get; set; }
}

public class BudgetBreakdownDto
{
    // Per person
    public decimal UserToStandCostPerPerson { get; set; }
    public decimal BusRouteCostPerPerson { get; set; }
    public decimal StandToDestCostPerPerson { get; set; }
    public decimal TransportTotalPerPerson { get; set; }
    public decimal FoodCostPerPerson { get; set; }
    public decimal TotalPerPerson { get; set; }

    // All people
    public decimal TransportTotalAllPeople { get; set; }
    public decimal FoodTotalAllPeople { get; set; }
    public decimal GrandTotal { get; set; }

    // Food level info
    public string FoodLevelName { get; set; } = string.Empty;
    public decimal FoodCostPerDayPerPerson { get; set; }
    public int Days { get; set; }
    public int People { get; set; }
}

public class SmartTripResultDto
{
    // Trip summary
    public string FromLocation { get; set; } = string.Empty;
    public string ToLocation { get; set; } = string.Empty;
    public string FromDistrict { get; set; } = string.Empty;
    public string ToDistrict { get; set; } = string.Empty;

    // Bus Stand info
    public string NearestFromStand { get; set; } = string.Empty;
    public double FromStandLat { get; set; }
    public double FromStandLng { get; set; }
    public string NearestToStand { get; set; } = string.Empty;
    public double ToStandLat { get; set; }
    public double ToStandLng { get; set; }

    // Distance & Time
    public double UserToStandKm { get; set; }
    public double StandToDestKm { get; set; }
    public double TotalTravelMinutes { get; set; }
    public string TotalTravelTime { get; set; } = string.Empty;

    // Budget
    public BudgetBreakdownDto Budget { get; set; } = new();

    // Route info
    public bool IsDirectRoute { get; set; }
    public bool SameDistrict { get; set; }

    // Step by step directions
    public List<TripStepDto> Steps { get; set; } = new();

    // Map links
    public string DestinationGoogleMapsUrl { get; set; } = string.Empty;
    public string FromStandGoogleMapsUrl { get; set; } = string.Empty;
    public string ToStandGoogleMapsUrl { get; set; } = string.Empty;

    // Place info
    public string PlaceName { get; set; } = string.Empty;
    public decimal PlaceEntryFee { get; set; }
}

// -------------------- Query --------------------

public class SmartTripCalculateQuery : IRequest<SmartTripResultDto>
{
    public double UserLat { get; set; }
    public double UserLng { get; set; }
    public int DestinationPlaceId { get; set; }
    public int People { get; set; } = 1;
    public int Days { get; set; } = 1;
    public FoodLevel FoodLevel { get; set; } = FoodLevel.Medium;
}

public class SmartTripCalculateQueryValidator : AbstractValidator<SmartTripCalculateQuery>
{
    public SmartTripCalculateQueryValidator()
    {
        RuleFor(x => x.UserLat).InclusiveBetween(-90, 90);
        RuleFor(x => x.UserLng).InclusiveBetween(-180, 180);
        RuleFor(x => x.DestinationPlaceId).GreaterThan(0);
        RuleFor(x => x.People).InclusiveBetween(1, 50);
        RuleFor(x => x.Days).InclusiveBetween(1, 30);
    }
}

// -------------------- Handler --------------------

public class SmartTripCalculateQueryHandler : IRequestHandler<SmartTripCalculateQuery, SmartTripResultDto>
{
    private readonly IApplicationDbContext _context;
    private const decimal PerKmLocalCost = 20m;
    private const double LocalSpeedKmh = 30.0;

    private static readonly Dictionary<FoodLevel, decimal> FoodCostPerDayPerPerson = new()
    {
        { FoodLevel.Low,    200m },
        { FoodLevel.Medium, 400m },
        { FoodLevel.VIP,    800m }
    };

    public SmartTripCalculateQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SmartTripResultDto> Handle(
        SmartTripCalculateQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Place load
        var place = await _context.Places
            .Include(p => p.District)
            .Include(p => p.Division)
            .FirstOrDefaultAsync(p => p.Id == request.DestinationPlaceId
                && p.ApprovalStatus == ApprovalStatus.Approved, cancellationToken);

        if (place == null)
            throw new NotFoundException("Place", request.DestinationPlaceId);

        var userCoord = new GeoCoordinate(request.UserLat, request.UserLng);
        var destCoord = new GeoCoordinate(place.Latitude, place.Longitude);

        // 2. All bus stands load
        var allBusStands = await _context.BusStands
            .Include(b => b.District)
            .ToListAsync(cancellationToken);

        // 3. Nearest FROM bus stand (user location থেকে)
        var nearestFromStand = allBusStands
            .OrderBy(b => userCoord.DistanceToInKm(
                new GeoCoordinate(b.Latitude, b.Longitude)))
            .First();

        // 4. Nearest TO bus stand (destination district এর)
        var nearestToStand = allBusStands
            .Where(b => b.DistrictId == place.DistrictId)
            .OrderBy(b => destCoord.DistanceToInKm(
                new GeoCoordinate(b.Latitude, b.Longitude)))
            .First();

        var fromDistrictId = nearestFromStand.DistrictId;
        var toDistrictId = place.DistrictId;
        var sameDistrict = fromDistrictId == toDistrictId;

        // 5. Distances calculate
        var userToStandKm = userCoord.DistanceToInKm(
            new GeoCoordinate(nearestFromStand.Latitude, nearestFromStand.Longitude));

        var standToDestKm = destCoord.DistanceToInKm(
            new GeoCoordinate(nearestToStand.Latitude, nearestToStand.Longitude));

        // 6. Bus Route খোঁজো
        DistrictBusRoute? busRoute = null;
        var isDirectRoute = false;

        if (!sameDistrict)
        {
            busRoute = await _context.DistrictBusRoutes
                .FirstOrDefaultAsync(r =>
                    (r.FromDistrictId == fromDistrictId && r.ToDistrictId == toDistrictId) ||
                    (r.IsBidirectional &&
                     r.FromDistrictId == toDistrictId &&
                     r.ToDistrictId == fromDistrictId),
                    cancellationToken);

            isDirectRoute = busRoute != null;

            // Direct route নেই → Haversine estimate
            if (busRoute == null)
            {
                var distKm = new GeoCoordinate(
                    nearestFromStand.Latitude, nearestFromStand.Longitude)
                    .DistanceToInKm(new GeoCoordinate(
                        nearestToStand.Latitude, nearestToStand.Longitude));

                busRoute = new DistrictBusRoute
                {
                    BusCost = Math.Round((decimal)distKm * 1.8m, 0),
                    BusTimeMinutes = (int)(distKm / 40.0 * 60)
                };
            }
        }

        // 7. Cost calculate
        var userToStandCost = Math.Round((decimal)userToStandKm * PerKmLocalCost, 0);
        var busRouteCost = busRoute?.BusCost ?? 0;
        var standToDestCost = Math.Round((decimal)standToDestKm * PerKmLocalCost, 0);
        var transportTotal = userToStandCost + busRouteCost + standToDestCost;

        var foodCostPerDayPerPerson = FoodCostPerDayPerPerson[request.FoodLevel];
        var foodPerPerson = foodCostPerDayPerPerson * request.Days;
        var foodTotal = foodPerPerson * request.People;

        var totalPerPerson = transportTotal + foodPerPerson;
        var grandTotal = (transportTotal * request.People) + foodTotal;

        // 8. Time calculate
        var userToStandMin = userToStandKm / LocalSpeedKmh * 60;
        var busRouteMin = busRoute?.BusTimeMinutes ?? 0;
        var standToDestMin = standToDestKm / LocalSpeedKmh * 60;
        var totalMinutes = userToStandMin + busRouteMin + standToDestMin;

        // 9. Format total time
        var totalHours = (int)(totalMinutes / 60);
        var remainingMin = (int)(totalMinutes % 60);
        var totalTimeStr = totalHours > 0
            ? $"{totalHours} hour{(totalHours > 1 ? "s" : "")} {remainingMin} min"
            : $"{remainingMin} min";

        // 10. Steps বানাও
        var steps = BuildSteps(
            userToStandKm, userToStandCost, userToStandMin,
            busRouteCost, busRouteMin,
            standToDestKm, standToDestCost, standToDestMin,
            nearestFromStand, nearestToStand,
            place.Name, sameDistrict, isDirectRoute);

        return new SmartTripResultDto
        {
            // Summary
            FromLocation = $"{nearestFromStand.District.Name} (near {nearestFromStand.Name})",
            ToLocation = place.Name,
            FromDistrict = nearestFromStand.District.Name,
            ToDistrict = place.District.Name,

            // Bus Stands
            NearestFromStand = nearestFromStand.Name,
            FromStandLat = nearestFromStand.Latitude,
            FromStandLng = nearestFromStand.Longitude,
            NearestToStand = nearestToStand.Name,
            ToStandLat = nearestToStand.Latitude,
            ToStandLng = nearestToStand.Longitude,

            // Distance & Time
            UserToStandKm = Math.Round(userToStandKm, 2),
            StandToDestKm = Math.Round(standToDestKm, 2),
            TotalTravelMinutes = Math.Round(totalMinutes, 0),
            TotalTravelTime = totalTimeStr,

            // Budget
            Budget = new BudgetBreakdownDto
            {
                UserToStandCostPerPerson = userToStandCost,
                BusRouteCostPerPerson = busRouteCost,
                StandToDestCostPerPerson = standToDestCost,
                TransportTotalPerPerson = transportTotal,
                FoodCostPerPerson = foodPerPerson,
                TotalPerPerson = totalPerPerson,
                TransportTotalAllPeople = transportTotal * request.People,
                FoodTotalAllPeople = foodTotal,
                GrandTotal = grandTotal,
                FoodLevelName = request.FoodLevel.ToString(),
                FoodCostPerDayPerPerson = foodCostPerDayPerPerson,
                Days = request.Days,
                People = request.People
            },

            // Route
            IsDirectRoute = isDirectRoute,
            SameDistrict = sameDistrict,

            // Steps
            Steps = steps,

            // Google Maps links
            DestinationGoogleMapsUrl =
                $"https://www.google.com/maps/search/?api=1&query={place.Latitude},{place.Longitude}",
            FromStandGoogleMapsUrl =
                $"https://www.google.com/maps/search/?api=1&query={nearestFromStand.Latitude},{nearestFromStand.Longitude}",
            ToStandGoogleMapsUrl =
                $"https://www.google.com/maps/search/?api=1&query={nearestToStand.Latitude},{nearestToStand.Longitude}",

            // Place
            PlaceName = place.Name,
            PlaceEntryFee = place.EntryFee
        };
    }

    private static List<TripStepDto> BuildSteps(
        double userToStandKm, decimal userToStandCost, double userToStandMin,
        decimal busRouteCost, int busRouteMin,
        double standToDestKm, decimal standToDestCost, double standToDestMin,
        BusStand fromStand, BusStand toStand,
        string placeName, bool sameDistrict, bool isDirectRoute)
    {
        var steps = new List<TripStepDto>();
        int stepNum = 1;

        // Step 1 — User → From Bus Stand
        steps.Add(new TripStepDto
        {
            StepNumber = stepNum++,
            TransportMode = "CNG / Rickshaw / Auto",
            TransportModeBn = "সিএনজি / রিকশা / অটো",
            From = "আপনার বর্তমান অবস্থান",
            To = fromStand.Name,
            Instruction = $"Go to {fromStand.Name} by CNG or Rickshaw. " +
                          $"Distance: {Math.Round(userToStandKm, 1)} km. " +
                          $"Approximate cost: {userToStandCost} BDT.",
            InstructionBn = $"CNG বা রিকশায় {fromStand.Name} যান। " +
                            $"দূরত্ব: {Math.Round(userToStandKm, 1)} কিমি। " +
                            $"আনুমানিক ভাড়া: {userToStandCost} টাকা।",
            DistanceKm = Math.Round(userToStandKm, 2),
            Cost = userToStandCost,
            TimeMinutes = Math.Round(userToStandMin, 0),
            GoogleMapsLink =
                $"https://www.google.com/maps/dir/?api=1&destination={fromStand.Latitude},{fromStand.Longitude}"
        });

        if (!sameDistrict)
        {
            // Step 2 — Bus Stand to Bus Stand
            var busHours = busRouteMin / 60;
            var busMin = busRouteMin % 60;
            var busTimeStr = busHours > 0 ? $"{busHours}h {busMin}min" : $"{busMin}min";

            steps.Add(new TripStepDto
            {
                StepNumber = stepNum++,
                TransportMode = isDirectRoute ? "Direct Bus" : "Bus (Estimated)",
                TransportModeBn = isDirectRoute ? "সরাসরি বাস" : "বাস (আনুমানিক)",
                From = fromStand.Name,
                To = toStand.Name,
                Instruction = $"Board a bus from {fromStand.Name} ({fromStand.District.Name}) " +
                              $"to {toStand.Name} ({toStand.District.Name}). " +
                              $"Fare: {busRouteCost} BDT. " +
                              $"Travel time: {busTimeStr}. " +
                              (isDirectRoute
                                  ? "This is a direct route."
                                  : "No direct route found. Fare is estimated."),
                InstructionBn = $"{fromStand.Name} ({fromStand.District.Name}) থেকে " +
                                $"{toStand.Name} ({toStand.District.Name}) যাওয়ার বাসে উঠুন। " +
                                $"ভাড়া: {busRouteCost} টাকা। " +
                                $"সময়: {busTimeStr}। " +
                                (isDirectRoute
                                    ? "এটি সরাসরি রুট।"
                                    : "সরাসরি রুট পাওয়া যায়নি। ভাড়া আনুমানিক।"),
                Cost = busRouteCost,
                TimeMinutes = busRouteMin,
                GoogleMapsLink =
                    $"https://www.google.com/maps/dir/?api=1" +
                    $"&origin={fromStand.Latitude},{fromStand.Longitude}" +
                    $"&destination={toStand.Latitude},{toStand.Longitude}"
            });
        }

        // Step 3 — To Bus Stand → Destination
        steps.Add(new TripStepDto
        {
            StepNumber = stepNum++,
            TransportMode = "CNG / Rickshaw / Auto",
            TransportModeBn = "সিএনজি / রিকশা / অটো",
            From = toStand.Name,
            To = placeName,
            Instruction = $"From {toStand.Name}, take a CNG or Rickshaw to {placeName}. " +
                          $"Distance: {Math.Round(standToDestKm, 1)} km. " +
                          $"Approximate cost: {standToDestCost} BDT. " +
                          $"You have reached your destination!",
            InstructionBn = $"{toStand.Name} থেকে CNG বা রিকশায় {placeName} যান। " +
                            $"দূরত্ব: {Math.Round(standToDestKm, 1)} কিমি। " +
                            $"আনুমানিক ভাড়া: {standToDestCost} টাকা। " +
                            $"আপনি গন্তব্যে পৌঁছে গেছেন!",
            DistanceKm = Math.Round(standToDestKm, 2),
            Cost = standToDestCost,
            TimeMinutes = Math.Round(standToDestMin, 0),
            GoogleMapsLink =
                $"https://www.google.com/maps/dir/?api=1" +
                $"&origin={toStand.Latitude},{toStand.Longitude}" +
                $"&destination=" + Uri.EscapeDataString(placeName)
        });

        return steps;
    }
}