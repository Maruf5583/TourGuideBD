using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;

namespace TourGuideBD.Domain.Entities.Trip;

public class ItineraryStop : BaseEntity
{
    public int ItineraryId { get; set; }
    public Itinerary Itinerary { get; set; } = null!;

    public int PlaceId { get; set; }
    public Place Place { get; set; } = null!;

    public int Order { get; set; } // sequence in the trip

    public int TransportTypeId { get; set; }
    public TransportType TransportType { get; set; } = null!;

    public double DistanceFromPreviousKm { get; set; }
    public decimal TransportCost { get; set; }
    public double TravelTimeMinutes { get; set; }
    public decimal EntryFeeAtThisStop { get; set; }
}