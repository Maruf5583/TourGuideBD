using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Commands.ReviewGuideApplication;

public class ReviewGuideApplicationCommand : IRequest<Unit>, IAuditableRequest
{
    public int ApplicationId { get; set; }

    /// <summary>
    /// true = Approve, false = Reject
    /// </summary>
    public bool IsApproved { get; set; }
    public string? AdminNote { get; set; }
    public string AdminUserId { get; set; } = string.Empty;

    public string ActionName => IsApproved ? "ApproveGuide" : "RejectGuide";
    public string EntityName => nameof(GuideApplication);
    public string? EntityId => ApplicationId.ToString();
}

public class ReviewGuideApplicationCommandValidator : AbstractValidator<ReviewGuideApplicationCommand>
{
    public ReviewGuideApplicationCommandValidator()
    {
        RuleFor(x => x.ApplicationId).GreaterThan(0);
        RuleFor(x => x.AdminNote).MaximumLength(500);
    }
}

public class ReviewGuideApplicationCommandHandler : IRequestHandler<ReviewGuideApplicationCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public ReviewGuideApplicationCommandHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Unit> Handle(ReviewGuideApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await _context.GuideApplications
            .FirstOrDefaultAsync(g => g.Id == request.ApplicationId, cancellationToken);

        if (application == null)
            throw new NotFoundException(nameof(GuideApplication), request.ApplicationId);

        if (application.Status == GuideApplicationStatus.Approved)
            throw new DomainValidationException("Application", "This application is already approved.");

        // ✅ Correct logic — IsApproved true হলে Approved, false হলে Rejected
        application.Status = request.IsApproved
            ? GuideApplicationStatus.Approved
            : GuideApplicationStatus.Rejected;

        application.AdminNote = request.AdminNote;
        application.ReviewedByUserId = request.AdminUserId;
        application.ReviewedAt = DateTime.UtcNow;
        application.UpdatedAt = DateTime.UtcNow;

        // ✅ শুধু Approved হলেই GuideProfile বানাবে
        if (request.IsApproved)
        {
            // Already profile আছে কিনা check করো
            var existingProfile = await _context.GuideProfiles
                .AnyAsync(g => g.UserId == application.UserId, cancellationToken);

            if (!existingProfile)
            {
                var profile = new GuideProfile
                {
                    UserId = application.UserId,
                    ApplicationId = application.Id,
                    FullName = application.FullName,
                    PhoneNumber = application.PhoneNumber,
                    ProfilePhotoUrl = application.ProfilePhotoUrl,
                    Bio = application.Bio,
                    Languages = application.Languages,
                    Specialities = application.Specialities,
                    ExperienceYears = application.ExperienceYears,
                    OperatingDistrictIds = application.OperatingDistrictIds,
                    Badge = GuideBadge.Verified,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.GuideProfiles.Add(profile);
            }

            // TourGuide role assign করো
            await _identityService.AddUserToRoleAsync(application.UserId, "TourGuide");
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}