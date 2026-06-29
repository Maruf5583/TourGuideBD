using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.DeleteTourPackage;

public class DeleteTourPackageCommand : IRequest<Unit>
{
    public int PackageId { get; set; }
    public string GuideUserId { get; set; } = string.Empty;
}

public class DeleteTourPackageCommandValidator : AbstractValidator<DeleteTourPackageCommand>
{
    public DeleteTourPackageCommandValidator()
    {
        RuleFor(x => x.PackageId).GreaterThan(0);
        RuleFor(x => x.GuideUserId).NotEmpty();
    }
}

public class DeleteTourPackageCommandHandler : IRequestHandler<DeleteTourPackageCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public DeleteTourPackageCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTourPackageCommand request, CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        var package = await _context.TourPackages
            .FirstOrDefaultAsync(p => p.Id == request.PackageId
                && p.GuideProfileId == guide.Id, cancellationToken);

        if (package == null)
            throw new NotFoundException("TourPackage", request.PackageId);

        // Active booking থাকলে delete করতে দেবো না
        var hasActiveBookings = await _context.Bookings
            .AnyAsync(b => b.TourPackageId == request.PackageId
                && (b.Status == BookingStatus.Pending
                    || b.Status == BookingStatus.Confirmed),
                cancellationToken);

        if (hasActiveBookings)
        {
            throw new DomainValidationException("PackageId",
                "Cannot delete package with active bookings. Deactivate it instead.");
        }

        // Soft delete — IsActive = false
        package.IsActive = false;
        package.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}