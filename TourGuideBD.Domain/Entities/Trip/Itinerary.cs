using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Users;

namespace TourGuideBD.Domain.Entities.Trip;

public class Itinerary : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public DateTime TripDate { get; set; }

    public decimal EstimatedTotalCost { get; set; }
    public decimal EstimatedFoodCost { get; set; }

    public ICollection<ItineraryStop> Stops { get; set; } = new List<ItineraryStop>();
}