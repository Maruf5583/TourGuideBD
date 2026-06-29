using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;

namespace TourGuideBD.Application.Features.Admin.Commands.FlushCache;

public class FlushCacheCommand : IRequest<Unit>, IAuditableRequest
{
    /// <summary>
    /// If null, flushes all "places:" cache keys (system-wide).
    /// If provided, flushes only "places:district:{districtId}:" prefix.
    /// </summary>
    public int? DistrictId { get; set; }

    public string ActionName => DistrictId.HasValue ? "FlushCache_District" : "FlushCache_All";
    public string EntityName => "Cache";
    public string? EntityId => DistrictId?.ToString();
}

public class FlushCacheCommandValidator : AbstractValidator<FlushCacheCommand>
{
    public FlushCacheCommandValidator()
    {
        RuleFor(x => x.DistrictId).GreaterThan(0).When(x => x.DistrictId.HasValue);
    }
}

public class FlushCacheCommandHandler : IRequestHandler<FlushCacheCommand, Unit>
{
    private readonly ICacheService _cacheService;

    public FlushCacheCommandHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task<Unit> Handle(FlushCacheCommand request, CancellationToken cancellationToken)
    {
        var prefix = request.DistrictId.HasValue
            ? $"places:district:{request.DistrictId.Value}:"
            : "places:";

        await _cacheService.RemoveByPrefixAsync(prefix, cancellationToken);

        return Unit.Value;
    }
}