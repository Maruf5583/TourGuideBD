using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.Realtime;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Admin.Commands.BroadcastMessage;

public class BroadcastMessageCommand : IRequest<int>, IAuditableRequest
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Null = system-wide broadcast
    /// </summary>
    public int? DistrictId { get; set; }

    /// <summary>
    /// Set internally from ICurrentUserService
    /// </summary>
    public string SentByUserId { get; set; } = string.Empty;

    public string ActionName => DistrictId.HasValue ? "BroadcastDistrict" : "BroadcastSystemWide";
    public string EntityName => nameof(Broadcast);
    public string? EntityId { get; set; }
}

public class BroadcastMessageCommandValidator : AbstractValidator<BroadcastMessageCommand>
{
    public BroadcastMessageCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.DistrictId).GreaterThan(0).When(x => x.DistrictId.HasValue);
    }
}

public class BroadcastMessageCommandHandler : IRequestHandler<BroadcastMessageCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public BroadcastMessageCommandHandler(IApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<int> Handle(BroadcastMessageCommand request, CancellationToken cancellationToken)
    {
        if (request.DistrictId.HasValue)
        {
            var districtExists = await _context.Districts.AnyAsync(d => d.Id == request.DistrictId.Value, cancellationToken);
            if (!districtExists)
            {
                throw new NotFoundException(nameof(District), request.DistrictId.Value);
            }
        }

        var broadcast = new Broadcast
        {
            Title = request.Title,
            Message = request.Message,
            DistrictId = request.DistrictId,
            SentByUserId = request.SentByUserId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Broadcasts.Add(broadcast);
        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.SendBroadcastAsync(request.DistrictId, request.Title, request.Message, cancellationToken);

        request.EntityId = broadcast.Id.ToString();

        return broadcast.Id;
    }
}