using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourGuideBD.Application.Features.Realtime.Commands.SendWeatherAlert;
using TourGuideBD.Application.Features.Realtime.Commands.UpdateLiveVisitorCount;
using TourGuideBD.Application.Features.Realtime.Queries.GetCrowdLevel;

namespace TourGuideBD.Api.Controllers.v1;

[ApiController]
[Route("api/v1/realtime")]
public class RealtimeController : ControllerBase
{
    private readonly IMediator _mediator;

    public RealtimeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get live visitor count & crowd level (Low/Medium/High) for a place
    /// </summary>
    [HttpGet("crowd-level/{placeId:int}")]
    public async Task<ActionResult<CrowdLevelDto>> GetCrowdLevel(int placeId)
    {
        var result = await _mediator.Send(new GetCrowdLevelQuery { PlaceId = placeId });
        return Ok(result);
    }

    /// <summary>
    /// Manually adjust live visitor count (Moderator/Admin) - e.g., checkout/decrement
    /// </summary>
    [Authorize(Policy = "ModeratorOnly")]
    [HttpPatch("live-visitor/{placeId:int}")]
    public async Task<IActionResult> UpdateLiveVisitorCount(int placeId, [FromBody] UpdateLiveVisitorCountCommand command)
    {
        if (placeId != command.PlaceId) return BadRequest("Route id and body id mismatch.");

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Send a district-specific weather alert (Moderator/Admin) - pushed via SignalR
    /// </summary>
    [Authorize(Policy = "ModeratorOnly")]
    [HttpPost("weather-alert")]
    public async Task<ActionResult<int>> SendWeatherAlert([FromBody] SendWeatherAlertCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }
}