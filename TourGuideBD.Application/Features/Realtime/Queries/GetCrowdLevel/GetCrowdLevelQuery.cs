using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Application.Features.Realtime.Queries.GetCrowdLevel;

public class CrowdLevelDto
{
    public int PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public int CurrentCount { get; set; }
    public string CrowdLevel { get; set; } = string.Empty;
    public DateTime LastUpdatedAt { get; set; }
}

public class GetCrowdLevelQuery : IRequest<CrowdLevelDto>
{
    public int PlaceId { get; set; }
}

public class GetCrowdLevelQueryValidator : AbstractValidator<GetCrowdLevelQuery>
{
    public GetCrowdLevelQueryValidator()
    {
        RuleFor(x => x.PlaceId).GreaterThan(0);
    }
}

public class GetCrowdLevelQueryHandler : IRequestHandler<GetCrowdLevelQuery, CrowdLevelDto>
{
    private readonly IApplicationDbContext _context;

    public GetCrowdLevelQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CrowdLevelDto> Handle(GetCrowdLevelQuery request, CancellationToken cancellationToken)
    {
        var liveVisitor = await _context.LiveVisitors
            .Include(l => l.Place)
            .FirstOrDefaultAsync(l => l.PlaceId == request.PlaceId, cancellationToken);

        if (liveVisitor == null)
        {
            var place = await _context.Places.FirstOrDefaultAsync(p => p.Id == request.PlaceId, cancellationToken);

            return new CrowdLevelDto
            {
                PlaceId = request.PlaceId,
                PlaceName = place?.Name ?? string.Empty,
                CurrentCount = 0,
                CrowdLevel = Domain.Enums.CrowdLevel.Low.ToString(),
                LastUpdatedAt = DateTime.UtcNow
            };
        }

        return new CrowdLevelDto
        {
            PlaceId = liveVisitor.PlaceId,
            PlaceName = liveVisitor.Place.Name,
            CurrentCount = liveVisitor.CurrentCount,
            CrowdLevel = liveVisitor.CrowdLevel.ToString(),
            LastUpdatedAt = liveVisitor.LastUpdatedAt
        };
    }
}