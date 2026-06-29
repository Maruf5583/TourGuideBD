using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Users.Commands.AddFavourite;
using TourGuideBD.Application.Features.Users.Commands.CheckIn;
using TourGuideBD.Application.Features.Users.Commands.RemoveFavourite;
using TourGuideBD.Application.Features.Users.Commands.SaveDistrict;
using TourGuideBD.Application.Features.Users.Commands.UnsaveDistrict;
using TourGuideBD.Application.Features.Users.Commands.UpdateProfile;
using TourGuideBD.Application.Features.Users.Commands.UploadAvatar;
using TourGuideBD.Application.Features.Users.Common;
using TourGuideBD.Application.Features.Users.Queries.GetCheckInTimeline;
using TourGuideBD.Application.Features.Users.Queries.GetFavourites;
using TourGuideBD.Application.Features.Users.Queries.GetProfile;
using TourGuideBD.Application.Features.Users.Queries.GetSavedDistricts;
using TourGuideBD.Application.Features.Users.Queries.GetVisitHistory;

namespace TourGuideBD.Api.Controllers.v1;

[ApiController]
[Authorize]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public UsersController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    private string CurrentUserId => _currentUserService.UserId!;

    /// <summary>
    /// Get current user's profile
    /// </summary>
    [HttpGet("me")]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        var result = await _mediator.Send(new GetProfileQuery { UserId = CurrentUserId });
        return Ok(result);
    }

    /// <summary>
    /// Update profile (full name)
    /// </summary>
    [HttpPut("me")]
    public async Task<ActionResult<UserProfileDto>> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        command.UserId = CurrentUserId;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Upload/replace avatar
    /// </summary>
    [HttpPost("me/avatar")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(object), 200)]
    public async Task<ActionResult<string>> UploadAvatar(IFormFile file)
    {
        var url = await _mediator.Send(new UploadAvatarCommand
        {
            UserId = CurrentUserId,
            File = file
        });
        return Ok(new { avatarUrl = url });
    }

    // ---------------- Favourites ----------------

    /// <summary>
    /// Get favourite places, grouped by district
    /// </summary>
    [HttpGet("me/favourites")]
    public async Task<ActionResult<List<FavouritesByDistrictDto>>> GetFavourites()
    {
        var result = await _mediator.Send(new GetFavouritesQuery { UserId = CurrentUserId });
        return Ok(result);
    }

    /// <summary>
    /// Add a place to favourites
    /// </summary>
    [HttpPost("me/favourites/{placeId:int}")]
    public async Task<IActionResult> AddFavourite(int placeId)
    {
        await _mediator.Send(new AddFavouriteCommand { UserId = CurrentUserId, PlaceId = placeId });
        return NoContent();
    }

    /// <summary>
    /// Remove a place from favourites
    /// </summary>
    [HttpDelete("me/favourites/{placeId:int}")]
    public async Task<IActionResult> RemoveFavourite(int placeId)
    {
        await _mediator.Send(new RemoveFavouriteCommand { UserId = CurrentUserId, PlaceId = placeId });
        return NoContent();
    }

    // ---------------- Visit History ----------------

    /// <summary>
    /// Get visit history (paginated)
    /// </summary>
    [HttpGet("me/visit-history")]
    public async Task<ActionResult<PaginatedList<VisitHistoryDto>>> GetVisitHistory(
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetVisitHistoryQuery
        {
            UserId = CurrentUserId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    // ---------------- Check-ins ----------------

    /// <summary>
    /// Check in to a place - adds to timeline, updates live visitor count
    /// </summary>
    [HttpPost("me/check-in/{placeId:int}")]
    public async Task<ActionResult<int>> CheckIn(int placeId, [FromBody] CheckInRequestBody body)
    {
        var id = await _mediator.Send(new CheckInCommand
        {
            UserId = CurrentUserId,
            PlaceId = placeId,
            Note = body.Note
        });

        return Ok(id);
    }

    /// <summary>
    /// Get checked-in places timeline (paginated)
    /// </summary>
    [HttpGet("me/check-ins")]
    public async Task<ActionResult<PaginatedList<CheckInDto>>> GetCheckInTimeline(
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetCheckInTimelineQuery
        {
            UserId = CurrentUserId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    // ---------------- Saved Districts ----------------

    /// <summary>
    /// Get saved districts (used for new-place & weather alert notifications)
    /// </summary>
    [HttpGet("me/saved-districts")]
    public async Task<ActionResult<List<SavedDistrictDto>>> GetSavedDistricts()
    {
        var result = await _mediator.Send(new GetSavedDistrictsQuery { UserId = CurrentUserId });
        return Ok(result);
    }

    /// <summary>
    /// Save a district for notifications
    /// </summary>
    [HttpPost("me/saved-districts/{districtId:int}")]
    public async Task<IActionResult> SaveDistrict(int districtId)
    {
        await _mediator.Send(new SaveDistrictCommand { UserId = CurrentUserId, DistrictId = districtId });
        return NoContent();
    }

    /// <summary>
    /// Remove a saved district
    /// </summary>
    [HttpDelete("me/saved-districts/{districtId:int}")]
    public async Task<IActionResult> UnsaveDistrict(int districtId)
    {
        await _mediator.Send(new UnsaveDistrictCommand { UserId = CurrentUserId, DistrictId = districtId });
        return NoContent();
    }
}

public class CheckInRequestBody
{
    public string? Note { get; set; }
}