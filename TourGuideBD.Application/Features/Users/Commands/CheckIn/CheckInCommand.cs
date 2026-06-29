using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.Realtime;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using CheckInEntity = TourGuideBD.Domain.Entities.Users.CheckIn;

namespace TourGuideBD.Application.Features.Users.Commands.CheckIn;

public class CheckInCommand : IRequest<int>
{
    public string UserId { get; set; } = string.Empty;
    public int PlaceId { get; set; }
    public string? Note { get; set; }
}

public class CheckInCommandValidator : AbstractValidator<CheckInCommand>
{
    public CheckInCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PlaceId).GreaterThan(0);
        RuleFor(x => x.Note).MaximumLength(300);
    }
}

public class CheckInCommandHandler : IRequestHandler<CheckInCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public CheckInCommandHandler(IApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<int> Handle(CheckInCommand request, CancellationToken cancellationToken)
    {
        var place = await _context.Places
            .FirstOrDefaultAsync(p => p.Id == request.PlaceId
                && p.ApprovalStatus == ApprovalStatus.Approved, cancellationToken);

        if (place == null)
        {
            throw new NotFoundException(nameof(Place), request.PlaceId);
        }

        var checkIn = new CheckInEntity
        {
            UserId = request.UserId,
            PlaceId = request.PlaceId,
            Note = request.Note,
            CheckedInAt = DateTime.UtcNow
        };

        _context.CheckIns.Add(checkIn);

        _context.VisitHistories.Add(new VisitHistory
        {
            UserId = request.UserId,
            PlaceId = request.PlaceId,
            VisitedAt = DateTime.UtcNow
        });

        var liveVisitor = await _context.LiveVisitors
            .FirstOrDefaultAsync(l => l.PlaceId == request.PlaceId, cancellationToken);

        if (liveVisitor == null)
        {
            liveVisitor = new LiveVisitor
            {
                PlaceId = request.PlaceId,
                CurrentCount = 1,
                CrowdLevel = CrowdLevel.Low,
                LastUpdatedAt = DateTime.UtcNow
            };
            _context.LiveVisitors.Add(liveVisitor);
        }
        else
        {
            liveVisitor.CurrentCount += 1;
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
            request.PlaceId,
            liveVisitor.CurrentCount,
            liveVisitor.CrowdLevel.ToString(),
            cancellationToken);

        return checkIn.Id;
    }
}