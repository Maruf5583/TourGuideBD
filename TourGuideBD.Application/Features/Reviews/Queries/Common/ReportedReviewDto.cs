namespace TourGuideBD.Application.Features.Reviews.Queries.Common;

public class ReportedReviewDto
{
    public int ReportId { get; set; }
    public int ReviewId { get; set; }

    public string Reason { get; set; } = string.Empty;
    public string ReportedByUserId { get; set; } = string.Empty;
    public string ReportedByUserName { get; set; } = string.Empty;

    public int Rating { get; set; }
    public string? CommentEn { get; set; }
    public string? CommentBn { get; set; }
    public string ReviewedUserName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}