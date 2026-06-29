using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.TripPlanner.Common;

namespace TourGuideBD.Application.Features.TripPlanner.Queries.EstimateTransportCost;

public class EstimateTransportCostQuery : IRequest<List<TransportCostOptionDto>>
{
    public double DistanceKm { get; set; }
}

public class EstimateTransportCostQueryValidator : AbstractValidator<EstimateTransportCostQuery>
{
    public EstimateTransportCostQueryValidator()
    {
        RuleFor(x => x.DistanceKm).GreaterThan(0);
    }
}

public class EstimateTransportCostQueryHandler : IRequestHandler<EstimateTransportCostQuery, List<TransportCostOptionDto>>
{
    private readonly IApplicationDbContext _context;

    public EstimateTransportCostQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TransportCostOptionDto>> Handle(EstimateTransportCostQuery request, CancellationToken cancellationToken)
    {
        var rates = await _context.TransportRates
            .Include(r => r.TransportType)
            .Where(r => r.IsActive)
            .ToListAsync(cancellationToken);

        return rates.Select(r =>
        {
            var rawCost = (decimal)request.DistanceKm * r.RatePerKm;
            var cost = Math.Max(rawCost, r.MinimumFare);

            var timeMinutes = r.AverageSpeedKmh > 0
                ? (request.DistanceKm / r.AverageSpeedKmh) * 60.0
                : 0;

            return new TransportCostOptionDto
            {
                TransportTypeId = r.TransportTypeId,
                TransportTypeName = r.TransportType.Name,
                EstimatedCost = Math.Round(cost, 2),
                EstimatedTimeMinutes = Math.Round(timeMinutes, 2)
            };
        })
        .OrderBy(x => x.EstimatedCost)
        .ToList();
    }
}