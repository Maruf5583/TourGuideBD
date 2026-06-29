using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Guide;

public class TourPackage : AuditableEntity
{
    public int GuideProfileId { get; set; }
    public GuideProfile GuideProfile { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public decimal PricePerPerson { get; set; }
    public int MaxPeople { get; set; }
    public int DurationDays { get; set; }

    // What's included
    public bool IncludesFood { get; set; }
    public bool IncludesTransport { get; set; }
    public bool IncludesAccommodation { get; set; }
    public string? AdditionalIncludes { get; set; }

    // Meeting point
    public string MeetingPoint { get; set; } = string.Empty;
    public double MeetingLat { get; set; }
    public double MeetingLng { get; set; }

    // Places included (JSON array of place IDs)
    public string PlaceIds { get; set; } = "[]";

    public bool IsActive { get; set; } = true;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<GuideAvailability> Availabilities { get; set; } = new List<GuideAvailability>();
}