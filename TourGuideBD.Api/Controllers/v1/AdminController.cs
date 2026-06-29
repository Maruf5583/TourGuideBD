using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Admin.Commands.AssignRole;
using TourGuideBD.Application.Features.Admin.Commands.BanUser;
using TourGuideBD.Application.Features.Admin.Commands.BroadcastMessage;
using TourGuideBD.Application.Features.Admin.Commands.FlushCache;
using TourGuideBD.Application.Features.Admin.Common;
using TourGuideBD.Application.Features.Admin.Queries.GetAnalytics;
using TourGuideBD.Application.Features.Admin.Queries.GetAuditLogs;
using TourGuideBD.Application.Features.Admin.Queries.GetPendingPlaces;
using TourGuideBD.Application.Features.Admin.Queries.GetUsers;
using TourGuideBD.Application.Features.Places.Queries.Common;

namespace TourGuideBD.Api.Controllers.v1;

[ApiController]
[Authorize(Policy = "AdminOnly")]
[Route("api/v1/admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public AdminController(IMediator mediator, ICurrentUserService currentUserService)
    { 
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    // ---------------- User Management ----------------

    /// <summary>
    /// List/search users (paginated)
    /// </summary>
    [HttpGet("users")]
    public async Task<ActionResult<PaginatedList<UserManagementDto>>> GetUsers(
        [FromQuery] string? search, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetUsersQuery { SearchTerm = search, PageNumber = pageNumber, PageSize = pageSize });
        return Ok(result);
    }

    /// <summary>
    /// Ban or unban a user
    /// </summary>
    [HttpPatch("users/{userId}/ban")]
    public async Task<IActionResult> BanUser(string userId, [FromBody] BanUserCommand command)
    {
        if (userId != command.UserId) return BadRequest("Route id and body id mismatch.");

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Assign or remove a role from a user (User, Moderator, Admin, TourGuide)
    /// </summary>
    [HttpPatch("users/{userId}/role")]
    public async Task<IActionResult> AssignRole(string userId, [FromBody] AssignRoleCommand command)
    {
        if (userId != command.UserId) return BadRequest("Route id and body id mismatch.");

        await _mediator.Send(command);
        return NoContent();
    }

    // ---------------- Place Approval Queue ----------------

    /// <summary>
    /// Get pending (unapproved) places submitted by users
    /// </summary>
    [HttpGet("places/pending")]
    public async Task<ActionResult<PaginatedList<PlaceListItemDto>>> GetPendingPlaces(
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetPendingPlacesQuery { PageNumber = pageNumber, PageSize = pageSize });
        return Ok(result);
    }

    // ---------------- Analytics ----------------

    /// <summary>
    /// District-wise visit/check-in/review stats dashboard
    /// </summary>
    [HttpGet("analytics")]
    public async Task<ActionResult<AnalyticsDashboardDto>> GetAnalytics()
    {
        var result = await _mediator.Send(new GetAnalyticsQuery());
        return Ok(result);
    }

    // ---------------- Broadcast ----------------

    /// <summary>
    /// Send a system-wide or district-specific broadcast (SignalR)
    /// </summary>
    [HttpPost("broadcast")]
    public async Task<ActionResult<int>> Broadcast([FromBody] BroadcastMessageCommand command)
    {
        command.SentByUserId = _currentUserService.UserId!;
        var id = await _mediator.Send(command);
        return Ok(id);
    }

    // ---------------- Cache Management ----------------

    /// <summary>
    /// Flush Redis cache - system-wide or per-district
    /// </summary>
    [HttpPost("cache/flush")]
    public async Task<IActionResult> FlushCache([FromBody] FlushCacheCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    // ---------------- Audit Logs ----------------

    /// <summary>
    /// Get audit trail (optionally filtered by entity name)
    /// </summary>
    [HttpGet("audit-logs")]
    public async Task<ActionResult<PaginatedList<AuditLogDto>>> GetAuditLogs(
        [FromQuery] string? entityName, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetAuditLogsQuery { EntityName = entityName, PageNumber = pageNumber, PageSize = pageSize });
        return Ok(result);
    }
}