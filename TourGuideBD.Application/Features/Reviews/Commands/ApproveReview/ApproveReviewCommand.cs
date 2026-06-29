using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Reviews;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Reviews.Commands.ApproveReview;

public class ApproveReviewCommand : IRequest<Unit>, IAuditableRequest
{
    public int ReviewId { get; set; }

    /// <summary>
    /// true = Approve, false = Reject
    /// </summary>
    public bool IsApproved { get; set; }

    public string? ModeratorNote { get; set; }

    /// <summary>
    /// Set internally from ICurrentUserService
    /// </summary>
    public string ModeratorId { get; set; } = string.Empty;

    public string ActionName => IsApproved ? "ApproveReview" : "RejectReview";
    public string EntityName => nameof(Review);
    public string? EntityId => ReviewId.ToString();
}

public class ApproveReviewCommandValidator : AbstractValidator<ApproveReviewCommand>
{
    public ApproveReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId).GreaterThan(0);
        RuleFor(x => x.ModeratorNote).MaximumLength(300);
    }
}

public class ApproveReviewCommandHandler : IRequestHandler<ApproveReviewCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ApproveReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ApproveReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await _context.Reviews
            .Include(r => r.Place)
            .FirstOrDefaultAsync(r => r.Id == request.ReviewId, cancellationToken);

        if (review == null)
        {
            throw new NotFoundException(nameof(Review), request.ReviewId);
        }

        review.Status = request.IsApproved ? ApprovalStatus.Approved : ApprovalStatus.Rejected;
        review.ModeratorId = request.ModeratorId;
        review.ModeratorNote = request.ModeratorNote;
        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        if (request.IsApproved)
        {
            await RecalculatePlaceRatingAsync(review.PlaceId, cancellationToken);
        }

        return Unit.Value;
    }

    private async Task RecalculatePlaceRatingAsync(int placeId, CancellationToken cancellationToken)
    {
        var place = await _context.Places.FirstOrDefaultAsync(p => p.Id == placeId, cancellationToken);
        if (place == null) return;

        var approvedReviews = await _context.Reviews
            .Where(r => r.PlaceId == placeId && r.Status == ApprovalStatus.Approved)
            .ToListAsync(cancellationToken);

        place.TotalReviews = approvedReviews.Count;
        place.AverageRating = approvedReviews.Count > 0
            ? Math.Round(approvedReviews.Average(r => r.Rating), 2)
            : 0;

        await _context.SaveChangesAsync(cancellationToken);
    }
}