using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Guide;

public class GuideApplication : AuditableEntity
{
    public string UserId { get; set; } = string.Empty;

    // Personal Info
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ProfilePhotoUrl { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }

    // Documents — NID Photo (number নেই)
    public string NidFrontPhotoUrl { get; set; } = string.Empty;
    public string NidBackPhotoUrl { get; set; } = string.Empty;

    // DOB Certificate (birth certificate photo)
    public string DobCertificatePhotoUrl { get; set; } = string.Empty;

    // Professional Info
    public int ExperienceYears { get; set; }
    public string Languages { get; set; } = string.Empty;
    public string Specialities { get; set; } = string.Empty;
    public string? CertificateUrl { get; set; }
    public string OperatingDistrictIds { get; set; } = "[]";

    // Status
    public GuideApplicationStatus Status { get; set; } = GuideApplicationStatus.Pending;
    public string? AdminNote { get; set; }
    public string? ReviewedByUserId { get; set; }
    public DateTime? ReviewedAt { get; set; }
}