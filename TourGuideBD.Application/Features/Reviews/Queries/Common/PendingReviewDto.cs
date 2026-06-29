namespace TourGuideBD.Application.Features.Reviews.Queries.Common;

public class PendingReviewDto
{
    public int Id { get; set; }
    public int PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;

    public int Rating { get; set; }
    public string? CommentEn { get; set; }
    public string? CommentBn { get; set; }

    public List<string> Photos { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}