using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.TripPlanner.Commands.CreateItinerary;
using TourGuideBD.Application.Features.TripPlanner.Common;
using TourGuideBD.Application.Features.TripPlanner.Queries.CalculateRoute;
using TourGuideBD.Application.Features.TripPlanner.Queries.EstimateTransportCost;
using TourGuideBD.Application.Features.TripPlanner.Queries.EstimateTravelTime;
using TourGuideBD.Application.Features.TripPlanner.Queries.GetItineraryById;
using TourGuideBD.Application.Features.TripPlanner.Queries.GetTripBudget;
using TourGuideBD.Application.Features.TripPlanner.Queries.SmartTripCalculate;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Api.Controllers.v1;

[ApiController]
[Route("api/v1/trip-planner")]
public class TripPlannerController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public TripPlannerController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Calculate route distance & duration between current location and destination (Mapbox)
    /// </summary>
    [HttpGet("route")]
    public async Task<ActionResult<RouteDto>> CalculateRoute([FromQuery] CalculateRouteQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Estimate transport cost across all modes (Bus, CNG, Train, Boat, Car, Bike) for a given distance
    /// </summary>
    [HttpGet("transport-cost")]
    public async Task<ActionResult<List<TransportCostOptionDto>>> EstimateTransportCost([FromQuery] EstimateTransportCostQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Estimate travel time per transport mode - traffic-aware for road-based modes when coordinates provided
    /// </summary>
    [HttpGet("travel-time")]
    public async Task<ActionResult<TravelTimeResultDto>> EstimateTravelTime([FromQuery] EstimateTravelTimeQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Total trip budget: transport + entry fee + food estimate for a single place
    /// </summary>
    [HttpGet("trip-budget")]
    public async Task<ActionResult<TripBudgetDto>> GetTripBudget([FromQuery] GetTripBudgetQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Multi-stop day trip planner with cumulative cost (authenticated users)
    /// </summary>
    [Authorize]
    [HttpPost("itinerary")]
    public async Task<ActionResult<ItineraryDto>> CreateItinerary([FromBody] CreateItineraryCommand command)
    {
        command.UserId = _currentUserService.UserId!;
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetItinerary), new { id = result.Id }, result);
    }

    /// <summary>
    /// Get a saved itinerary by id (owner only)
    /// </summary>
    [Authorize]
    [HttpGet("itinerary/{id:int}")]
    public async Task<ActionResult<ItineraryDto>> GetItinerary(int id)
    {
        var result = await _mediator.Send(new GetItineraryByIdQuery
        {
            ItineraryId = id,
            UserId = _currentUserService.UserId!
        });

        return Ok(result);
    }


    /// <summary>
    /// Smart trip calculator — Bus stand based district route calculation
    /// User location → Nearest Bus Stand → District Route → Destination
    /// </summary>
    [HttpGet("smart-calculate")]
    public async Task<ActionResult<SmartTripResultDto>> SmartCalculate(
        [FromQuery] double userLat,
        [FromQuery] double userLng,
        [FromQuery] int destinationPlaceId,
        [FromQuery] int people = 1,
        [FromQuery] int days = 1,
        [FromQuery] FoodLevel foodLevel = FoodLevel.Medium)
    {
        var result = await _mediator.Send(new SmartTripCalculateQuery
        {
            UserLat = userLat,
            UserLng = userLng,
            DestinationPlaceId = destinationPlaceId,
            People = people,
            Days = days,
            FoodLevel = foodLevel
        });

        return Ok(result);
    }
}