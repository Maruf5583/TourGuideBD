using TourGuideBD.Domain.Entities.Common;

namespace TourGuideBD.Domain.Entities.Reviews;

public class ReviewPhoto : BaseEntity
{
    public int ReviewId { get; set; }
    public Review Review { get; set; } = null!;

    public string Url { get; set; } = string.Empty;
}