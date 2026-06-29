using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Commands.UpdateLiveLocation;

public class UpdateLiveLocationCommand : IRequest<Unit>
{
    public string GuideUserId { get; set; } = string.Empty;
    public int BookingId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsSharing { get; set; } = true;
}

public class UpdateLiveLocationCommandValidator : AbstractValidator<UpdateLiveLocationCommand>
{
    public UpdateLiveLocationCommandValidator()
    {
        RuleFor(x => x.BookingId).GreaterThan(0);
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
    }
}

public class UpdateLiveLocationCommandHandler : IRequestHandler<UpdateLiveLocationCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public UpdateLiveLocationCommandHandler(
        IApplicationDbContext context,
        INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<Unit> Handle(
        UpdateLiveLocationCommand request,
        CancellationToken cancellationToken)
    {
        // Booking verify করো
        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b =>
                b.Id == request.BookingId &&
                b.TourPackage.GuideProfile.UserId == request.GuideUserId,
                cancellationToken);

        if (booking == null)
        {
            throw new NotFoundException("Booking", request.BookingId);
        }

        var location = await _context.GuideLiveLocations
            .FirstOrDefaultAsync(g => g.BookingId == request.BookingId, cancellationToken);

        if (location == null)
        {
            location = new GuideLiveLocation
            {
                BookingId = request.BookingId,
                GuideUserId = request.GuideUserId,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                IsSharing = request.IsSharing,
                LastUpdatedAt = DateTime.UtcNow
            };
            _context.GuideLiveLocations.Add(location);
        }
        else
        {
            location.Latitude = request.Latitude;
            location.Longitude = request.Longitude;
            location.IsSharing = request.IsSharing;
            location.LastUpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        // SignalR এ push করো — User real-time location পাবে
        await _notificationService.SendGuideLocationUpdateAsync(
            request.BookingId,
            request.Latitude,
            request.Longitude,
            request.IsSharing,
            cancellationToken);

        return Unit.Value;
    }
}