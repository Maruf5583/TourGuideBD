using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.RemoveGuide;

public class RemoveGuideCommand : IRequest<Unit>, IAuditableRequest
{
    public int GuideProfileId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string AdminUserId { get; set; } = string.Empty;

    public string ActionName => "RemoveGuide";
    public string EntityName => "GuideProfile";
    public string? EntityId => GuideProfileId.ToString();
}

public class RemoveGuideCommandValidator : AbstractValidator<RemoveGuideCommand>
{
    public RemoveGuideCommandValidator()
    {
        RuleFor(x => x.GuideProfileId).GreaterThan(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}

public class RemoveGuideCommandHandler : IRequestHandler<RemoveGuideCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public RemoveGuideCommandHandler(
        IApplicationDbContext context,
        IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task<Unit> Handle(RemoveGuideCommand request, CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .Include(g => g.TourPackages)
            .FirstOrDefaultAsync(g => g.Id == request.GuideProfileId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideProfileId);

        // Active bookings আছে কিনা check করো
        var hasActiveBookings = await _context.Bookings
            .AnyAsync(b =>
                b.TourPackage.GuideProfileId == request.GuideProfileId &&
                (b.Status == BookingStatus.Pending ||
                 b.Status == BookingStatus.Confirmed),
                cancellationToken);

        if (hasActiveBookings)
        {
            throw new DomainValidationException("GuideProfileId",
                "Cannot remove guide with active bookings. Resolve bookings first.");
        }

        // Guide deactivate করো
        guide.IsActive = false;
        guide.UpdatedAt = DateTime.UtcNow;

        // সব packages deactivate করো
        foreach (var package in guide.TourPackages)
        {
            package.IsActive = false;
            package.UpdatedAt = DateTime.UtcNow;
        }

        // TourGuide role remove করো
        await _identityService.RemoveUserFromRoleAsync(guide.UserId, "TourGuide");

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}