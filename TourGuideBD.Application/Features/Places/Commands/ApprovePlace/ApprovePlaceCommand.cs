using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Places.Commands.ApprovePlace;

public class ApprovePlaceCommand : IRequest<Unit>, IAuditableRequest
{
    public int PlaceId { get; set; }

    /// <summary>
    /// true = Approve, false = Reject
    /// </summary>
    public bool IsApproved { get; set; }

    public string ActionName => IsApproved ? "ApprovePlace" : "RejectPlace";
    public string EntityName => nameof(Place);
    public string? EntityId => PlaceId.ToString();
}

public class ApprovePlaceCommandValidator : AbstractValidator<ApprovePlaceCommand>
{
    public ApprovePlaceCommandValidator()
    {
        RuleFor(x => x.PlaceId).GreaterThan(0);
    }
}

public class ApprovePlaceCommandHandler : IRequestHandler<ApprovePlaceCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public ApprovePlaceCommandHandler(IApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<Unit> Handle(ApprovePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _context.Places.FirstOrDefaultAsync(p => p.Id == request.PlaceId, cancellationToken);

        if (place == null)
        {
            throw new NotFoundException(nameof(Place), request.PlaceId);
        }

        place.ApprovalStatus = request.IsApproved ? ApprovalStatus.Approved : ApprovalStatus.Rejected;
        place.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        if (request.IsApproved)
        {
            // Notify users who saved this district about the new approved place
            await _notificationService.SendNewPlaceNotificationAsync(place.DistrictId, place.Id, place.Name, cancellationToken);
        }

        return Unit.Value;
    }
}