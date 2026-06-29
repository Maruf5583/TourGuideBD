using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Domain.Entities.Reviews;

public class Review : AuditableEntity
{
    public int PlaceId { get; set; }
    public Place Place { get; set; } = null!;

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int Rating { get; set; } // 1-5

    public string? CommentEn { get; set; }
    public string? CommentBn { get; set; }

    public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending;

    public string? ModeratorId { get; set; }
    public string? ModeratorNote { get; set; }

    public ICollection<ReviewPhoto> Photos { get; set; } = new List<ReviewPhoto>();
    public ICollection<ReviewReport> Reports { get; set; } = new List<ReviewReport>();
}