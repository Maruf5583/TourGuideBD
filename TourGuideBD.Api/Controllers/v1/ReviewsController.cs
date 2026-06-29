using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Reviews.Commands.ApproveReview;
using TourGuideBD.Application.Features.Reviews.Commands.CreateReview;
using TourGuideBD.Application.Features.Reviews.Commands.ReportReview;
using TourGuideBD.Application.Features.Reviews.Commands.ResolveReviewReport;
using TourGuideBD.Application.Features.Reviews.Commands.UploadReviewPhotos;
using TourGuideBD.Application.Features.Reviews.Queries.Common;
using TourGuideBD.Application.Features.Reviews.Queries.GetPendingReviews;
using TourGuideBD.Application.Features.Reviews.Queries.GetReportedReviews;
using TourGuideBD.Application.Features.Reviews.Queries.GetReviewsByPlace;

namespace TourGuideBD.Api.Controllers.v1;

[ApiController]
[Route("api/v1/reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public ReviewsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Get approved reviews for a place (paginated)
    /// </summary>
    [HttpGet("place/{placeId:int}")]
    public async Task<ActionResult<PaginatedList<ReviewDto>>> GetByPlace(
        int placeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetReviewsByPlaceQuery
        {
            PlaceId = placeId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// Upload review photos (max 5)
    /// </summary>
    [Authorize]
    [HttpPost("photos")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(List<string>), 200)]
    public async Task<ActionResult<List<string>>> UploadPhotos(List<IFormFile> files)
    {
        var result = await _mediator.Send(new UploadReviewPhotosCommand { Files = files });
        return Ok(result);
    }
    /// <summary>
    /// Submit a review (Star rating 1-5, Bangla/English text, max 5 photos) - goes to moderation queue
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateReviewCommand command)
    {
        command.UserId = _currentUserService.UserId!;
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByPlace), new { placeId = command.PlaceId }, id);
    }

    /// <summary>
    /// Report a review as fake/spam
    /// </summary>
    [Authorize]
    [HttpPost("{reviewId:int}/report")]
    public async Task<ActionResult<int>> Report(int reviewId, [FromBody] ReportReviewCommand command)
    {
        if (reviewId != command.ReviewId) return BadRequest("Route id and body id mismatch.");

        command.ReportedByUserId = _currentUserService.UserId!;
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    // ---------------- Moderator Endpoints ----------------

    /// <summary>
    /// Get pending reviews queue (Moderator/Admin)
    /// </summary>
    [Authorize(Policy = "ModeratorOnly")]
    [HttpGet("pending")]
    public async Task<ActionResult<PaginatedList<PendingReviewDto>>> GetPending(
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetPendingReviewsQuery { PageNumber = pageNumber, PageSize = pageSize });
        return Ok(result);
    }

    /// <summary>
    /// Approve or reject a pending review (Moderator/Admin)
    /// </summary>
    [Authorize(Policy = "ModeratorOnly")]
    [HttpPatch("{reviewId:int}/approval")]
    public async Task<IActionResult> SetApproval(int reviewId, [FromBody] ApproveReviewCommand command)
    {
        if (reviewId != command.ReviewId) return BadRequest("Route id and body id mismatch.");

        command.ModeratorId = _currentUserService.UserId!;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Get reported (flagged) reviews queue (Moderator/Admin)
    /// </summary>
    [Authorize(Policy = "ModeratorOnly")]
    [HttpGet("reports")]
    public async Task<ActionResult<PaginatedList<ReportedReviewDto>>> GetReports(
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetReportedReviewsQuery { PageNumber = pageNumber, PageSize = pageSize });
        return Ok(result);
    }

    /// <summary>
    /// Resolve a reported review - take action (reject review) or dismiss (Moderator/Admin)
    /// </summary>
    [Authorize(Policy = "ModeratorOnly")]
    [HttpPatch("reports/{reportId:int}/resolve")]
    public async Task<IActionResult> ResolveReport(int reportId, [FromBody] ResolveReviewReportCommand command)
    {
        if (reportId != command.ReportId) return BadRequest("Route id and body id mismatch.");

        command.ResolvedByUserId = _currentUserService.UserId!;
        await _mediator.Send(command);
        return NoContent();
    }
}