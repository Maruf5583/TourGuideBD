using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Guide;

public class GuideReview : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public int GuideProfileId { get; set; }
    public GuideProfile GuideProfile { get; set; } = null!;
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    // Ratings (1-5 each)
    public int PunctualityRating { get; set; }
    public int KnowledgeRating { get; set; }
    public int CommunicationRating { get; set; }
    public int SafetyRating { get; set; }
    public int ValueRating { get; set; }
    public int OverallRating { get; set; }

    public string? Comment { get; set; }
}