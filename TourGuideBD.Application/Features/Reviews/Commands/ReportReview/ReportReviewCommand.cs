using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Reviews;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using ValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Reviews.Commands.ReportReview;

public class ReportReviewCommand : IRequest<int>, IAuditableRequest
{
    public int ReviewId { get; set; }
    public string Reason { get; set; } = string.Empty;

    /// <summary>
    /// Set internally from ICurrentUserService
    /// </summary>
    public string ReportedByUserId { get; set; } = string.Empty;

    public string ActionName => "ReportReview";
    public string EntityName => nameof(ReviewReport);
    public string? EntityId { get; set; }
}

public class ReportReviewCommandValidator : AbstractValidator<ReportReviewCommand>
{
    public ReportReviewCommandValidator()
    {
        RuleFor(x => x.ReviewId).GreaterThan(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(300);
    }
}

public class ReportReviewCommandHandler : IRequestHandler<ReportReviewCommand, int>
{
    private readonly IApplicationDbContext _context;

    public ReportReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(ReportReviewCommand request, CancellationToken cancellationToken)
    {
        var reviewExists = await _context.Reviews.AnyAsync(r => r.Id == request.ReviewId, cancellationToken);
        if (!reviewExists)
        {
            throw new NotFoundException(nameof(Review), request.ReviewId);
        }

        var alreadyReported = await _context.ReviewReports
            .AnyAsync(r => r.ReviewId == request.ReviewId
                && r.ReportedByUserId == request.ReportedByUserId
                && r.Status == ReportStatus.Open, cancellationToken);

        if (alreadyReported)
        {
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new(nameof(request.ReviewId), "You have already reported this review and it is pending action.")
            });
        }

        var report = new ReviewReport
        {
            ReviewId = request.ReviewId,
            ReportedByUserId = request.ReportedByUserId,
            Reason = request.Reason,
            Status = ReportStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        _context.ReviewReports.Add(report);
        await _context.SaveChangesAsync(cancellationToken);

        request.EntityId = report.Id.ToString();

        return report.Id;
    }
}