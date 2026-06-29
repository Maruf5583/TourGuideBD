using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Guide;

public class GuideAvailability : BaseEntity
{
    public int TourPackageId { get; set; }
    public TourPackage TourPackage { get; set; } = null!;

    public DateTime AvailableDate { get; set; }
    public int MaxBookings { get; set; }
    public int CurrentBookings { get; set; } = 0;
    public bool IsAvailable { get; set; } = true;
}