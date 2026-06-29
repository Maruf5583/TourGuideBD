using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Reviews;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Reviews.Commands.ResolveReviewReport;

public class ResolveReviewReportCommand : IRequest<Unit>, IAuditableRequest
{
    public int ReportId { get; set; }

    /// <summary>
    /// ActionTaken (remove the review), or Dismissed (no action)
    /// </summary>
    public bool TakeAction { get; set; }

    public string? ResolutionNote { get; set; }

    /// <summary>
    /// Set internally from ICurrentUserService
    /// </summary>
    public string ResolvedByUserId { get; set; } = string.Empty;

    public string ActionName => TakeAction ? "ResolveReport_ActionTaken" : "ResolveReport_Dismissed";
    public string EntityName => nameof(ReviewReport);
    public string? EntityId => ReportId.ToString();
}

public class ResolveReviewReportCommandValidator : AbstractValidator<ResolveReviewReportCommand>
{
    public ResolveReviewReportCommandValidator()
    {
        RuleFor(x => x.ReportId).GreaterThan(0);
        RuleFor(x => x.ResolutionNote).MaximumLength(300);
    }
}

public class ResolveReviewReportCommandHandler : IRequestHandler<ResolveReviewReportCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public ResolveReviewReportCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(ResolveReviewReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _context.ReviewReports
            .Include(r => r.Review)
            .FirstOrDefaultAsync(r => r.Id == request.ReportId, cancellationToken);

        if (report == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Reviews.ReviewReport), request.ReportId);
        }

        report.Status = request.TakeAction ? ReportStatus.ActionTaken : ReportStatus.Dismissed;
        report.ResolvedByUserId = request.ResolvedByUserId;
        report.ResolutionNote = request.ResolutionNote;
        report.UpdatedAt = DateTime.UtcNow;

        if (request.TakeAction)
        {
            // Mark the reported review as Rejected (effectively hidden from public)
            report.Review.Status = ApprovalStatus.Rejected;
            report.Review.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}