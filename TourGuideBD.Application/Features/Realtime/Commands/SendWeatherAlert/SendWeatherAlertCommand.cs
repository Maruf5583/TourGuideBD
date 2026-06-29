using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.Realtime;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Realtime.Commands.SendWeatherAlert;

public class SendWeatherAlertCommand : IRequest<int>, IAuditableRequest
{
    public int DistrictId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Info, Warning, Severe
    /// </summary>
    public string Severity { get; set; } = "Info";

    public DateTime ValidUntil { get; set; }

    public string ActionName => "SendWeatherAlert";
    public string EntityName => nameof(WeatherAlert);
    public string? EntityId { get; set; }
}

public class SendWeatherAlertCommandValidator : AbstractValidator<SendWeatherAlertCommand>
{
    private static readonly string[] AllowedSeverities = { "Info", "Warning", "Severe" };

    public SendWeatherAlertCommandValidator()
    {
        RuleFor(x => x.DistrictId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Message).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Severity).Must(s => AllowedSeverities.Contains(s))
            .WithMessage($"Severity must be one of: {string.Join(", ", AllowedSeverities)}");
        RuleFor(x => x.ValidUntil).GreaterThan(DateTime.UtcNow);
    }
}

public class SendWeatherAlertCommandHandler : IRequestHandler<SendWeatherAlertCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly INotificationService _notificationService;

    public SendWeatherAlertCommandHandler(IApplicationDbContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<int> Handle(SendWeatherAlertCommand request, CancellationToken cancellationToken)
    {
        var districtExists = await _context.Districts.AnyAsync(d => d.Id == request.DistrictId, cancellationToken);
        if (!districtExists)
        {
            throw new NotFoundException(nameof(District), request.DistrictId);
        }

        var alert = new WeatherAlert
        {
            DistrictId = request.DistrictId,
            Title = request.Title,
            Message = request.Message,
            Severity = request.Severity,
            ValidUntil = request.ValidUntil,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.WeatherAlerts.Add(alert);
        await _context.SaveChangesAsync(cancellationToken);

        await _notificationService.SendWeatherAlertAsync(
            request.DistrictId, request.Title, request.Message, request.Severity, cancellationToken);

        request.EntityId = alert.Id.ToString();

        return alert.Id;
    }
}