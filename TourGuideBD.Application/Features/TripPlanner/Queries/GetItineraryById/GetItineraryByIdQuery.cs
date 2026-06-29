using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.TripPlanner.Common;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.TripPlanner.Queries.GetItineraryById;

public class GetItineraryByIdQuery : IRequest<ItineraryDto>
{
    public int ItineraryId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class GetItineraryByIdQueryValidator : AbstractValidator<GetItineraryByIdQuery>
{
    public GetItineraryByIdQueryValidator()
    {
        RuleFor(x => x.ItineraryId).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class GetItineraryByIdQueryHandler : IRequestHandler<GetItineraryByIdQuery, ItineraryDto>
{
    private readonly IApplicationDbContext _context;

    public GetItineraryByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ItineraryDto> Handle(GetItineraryByIdQuery request, CancellationToken cancellationToken)
    {
        var itinerary = await _context.Itineraries
            .Include(i => i.Stops).ThenInclude(s => s.Place)
            .Include(i => i.Stops).ThenInclude(s => s.TransportType)
            .FirstOrDefaultAsync(i => i.Id == request.ItineraryId && i.UserId == request.UserId, cancellationToken);

        if (itinerary == null)
        {
            throw new NotFoundException(nameof(Domain.Entities.Trip.Itinerary), request.ItineraryId);
        }

        return new ItineraryDto
        {
            Id = itinerary.Id,
            Title = itinerary.Title,
            TripDate = itinerary.TripDate,
            EstimatedTotalCost = itinerary.EstimatedTotalCost,
            EstimatedFoodCost = itinerary.EstimatedFoodCost,
            Stops = itinerary.Stops.OrderBy(s => s.Order).Select(s => new ItineraryStopDto
            {
                PlaceId = s.PlaceId,
                PlaceName = s.Place.Name,
                Order = s.Order,
                TransportTypeId = s.TransportTypeId,
                TransportTypeName = s.TransportType.Name,
                DistanceFromPreviousKm = s.DistanceFromPreviousKm,
                TransportCost = s.TransportCost,
                TravelTimeMinutes = s.TravelTimeMinutes,
                EntryFeeAtThisStop = s.EntryFeeAtThisStop
            }).ToList()
        };
    }
}