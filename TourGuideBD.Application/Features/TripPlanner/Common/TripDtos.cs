namespace TourGuideBD.Application.Features.TripPlanner.Common;

public class RouteDto
{
    public double DistanceKm { get; set; }
    public double DurationMinutes { get; set; }
    public string? Geometry { get; set; }
}

public class TransportCostOptionDto
{
    public int TransportTypeId { get; set; }
    public string TransportTypeName { get; set; } = string.Empty;
    public decimal EstimatedCost { get; set; }
    public double EstimatedTimeMinutes { get; set; }
}

public class TripBudgetDto
{
    public double DistanceKm { get; set; }
    public decimal TransportCost { get; set; }
    public decimal EntryFee { get; set; }
    public decimal EstimatedFoodCost { get; set; }
    public decimal TotalCost { get; set; }
    public double EstimatedTravelTimeMinutes { get; set; }
    public string TransportTypeName { get; set; } = string.Empty;
}

public class ItineraryStopDto
{
    public int PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public int Order { get; set; }
    public int TransportTypeId { get; set; }
    public string TransportTypeName { get; set; } = string.Empty;
    public double DistanceFromPreviousKm { get; set; }
    public decimal TransportCost { get; set; }
    public double TravelTimeMinutes { get; set; }
    public decimal EntryFeeAtThisStop { get; set; }
}

public class ItineraryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime TripDate { get; set; }
    public decimal EstimatedTotalCost { get; set; }
    public decimal EstimatedFoodCost { get; set; }
    public List<ItineraryStopDto> Stops { get; set; } = new();
}