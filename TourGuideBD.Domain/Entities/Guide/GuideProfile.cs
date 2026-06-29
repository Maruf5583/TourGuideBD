using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Guide;

public class GuideProfile : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;
    public int ApplicationId { get; set; }
    public GuideApplication Application { get; set; } = null!;

    // Profile Info
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProfilePhotoUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public string Specialities { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string OperatingDistrictIds { get; set; } = "[]";

    // Badge
    public GuideBadge Badge { get; set; } = GuideBadge.Verified;

    // Stats
    public double AverageRating { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;
    public int TotalToursCompleted { get; set; } = 0;

    // Status
    public bool IsActive { get; set; } = true;

    // Stripe
    public string? StripeAccountId { get; set; }

    // Navigation
    public ICollection<TourPackage> TourPackages { get; set; } = new List<TourPackage>();
    public ICollection<GuideReview> Reviews { get; set; } = new List<GuideReview>();
}