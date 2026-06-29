using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Realtime;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Realtime.Commands.UpdateLiveVisitorCount;

public class UpdateLiveVisitorCountCommand : IRequest<Unit>
{
    public int PlaceId { get; set; }

    /// <summary>
    /// Delta to apply to current count (can be negative for checkout/decrement)
    /// </summary>
    public int Delta { get; set; }
}

public class UpdateLiveVisitorCountCommandValidator : AbstractValidator<UpdateLiveVisitorCountCommand>
{
    public UpdateLiveVisitorCountCommandValidator()
    {
        RuleFor(x => x.PlaceId).GreaterThan(0);
    }
}

public class UpdateLiveVisitorCountCommandHandler : IRequestHandler<UpdateLiveVisitorCountCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public UpdateLiveVisitorCountCommandHandler(IApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<Unit> Handle(UpdateLiveVisitorCountCommand request, CancellationToken cancellationToken)
    {
        var placeExists = await _context.Places.AnyAsync(p => p.Id == request.PlaceId, cancellationToken);
        if (!placeExists)
        {
            throw new NotFoundException("Place", request.PlaceId);
        }

        var liveVisitor = await _context.LiveVisitors.FirstOrDefaultAsync(l => l.PlaceId == request.PlaceId, cancellationToken);

        if (liveVisitor == null)
        {
            liveVisitor = new LiveVisitor
            {
                PlaceId = request.PlaceId,
                CurrentCount = Math.Max(0, request.Delta),
                LastUpdatedAt = DateTime.UtcNow
            };
            _context.LiveVisitors.Add(liveVisitor);
        }
        else
        {
            liveVisitor.CurrentCount = Math.Max(0, liveVisitor.CurrentCount + request.Delta);
            liveVisitor.LastUpdatedAt = DateTime.UtcNow;
        }

        liveVisitor.CrowdLevel = liveVisitor.CurrentCount switch
        {
            <= 10 => CrowdLevel.Low,
            <= 30 => CrowdLevel.Medium,
            _ => CrowdLevel.High
        };

        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.SendLiveVisitorUpdateAsync(
            request.PlaceId, liveVisitor.CurrentCount, liveVisitor.CrowdLevel.ToString(), cancellationToken);

        return Unit.Value;
    }
}