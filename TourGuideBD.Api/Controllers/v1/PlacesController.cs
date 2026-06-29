using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Places.Commands.ApprovePlace;
using TourGuideBD.Application.Features.Places.Commands.CreatePlace;
using TourGuideBD.Application.Features.Places.Commands.DeletePlace;
using TourGuideBD.Application.Features.Places.Commands.UpdatePlace;
using TourGuideBD.Application.Features.Places.Commands.UploadPlacePhotos;
using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Application.Features.Places.Queries.FilterByCategory;
using TourGuideBD.Application.Features.Places.Queries.GetAllPlaces;
using TourGuideBD.Application.Features.Places.Queries.GetNearbyPlaces;
using TourGuideBD.Application.Features.Places.Queries.GetPlaceDetail;
using TourGuideBD.Application.Features.Places.Queries.GetPlacesByDistrict;
using TourGuideBD.Application.Features.Places.Queries.SearchPlaces;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Api.Controllers.v1;

[ApiController]
[Route("api/v1/places")]
public class PlacesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public PlacesController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// District-wise approved place listing (paginated)
    /// </summary>
    [HttpGet("by-district/{districtId:int}")]
    public async Task<ActionResult<PaginatedList<PlaceListItemDto>>> GetByDistrict(
        int districtId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetPlacesByDistrictQuery
        {
            DistrictId = districtId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// Filter places by category (Beach, Hill, Forest, Historical, Religious, Waterfall)
    /// </summary>
    [HttpGet("by-category/{category}")]
    public async Task<ActionResult<PaginatedList<PlaceListItemDto>>> GetByCategory(
        PlaceCategoryEnum category, [FromQuery] int? districtId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new FilterPlacesByCategoryQuery
        {
            Category = category,
            DistrictId = districtId,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// Place detail page - photos, description, entry fee, best season, tags, categories
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlaceDetailDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetPlaceDetailQuery { PlaceId = id });
        return Ok(result);
    }

    /// <summary>
    /// GPS-based nearby search. radiusKm must be 5, 10, or 20.
    /// </summary>
    [HttpGet("nearby")]
    public async Task<ActionResult<List<PlaceListItemDto>>> GetNearby(
        [FromQuery] double lat, [FromQuery] double lng, [FromQuery] int radiusKm = 5)
    {
        var result = await _mediator.Send(new GetNearbyPlacesQuery
        {
            Latitude = lat,
            Longitude = lng,
            RadiusKm = radiusKm
        });

        return Ok(result);
    }

    /// <summary>
    /// Full-text search by name, tag, district, division
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<PaginatedList<PlaceListItemDto>>> Search(
        [FromQuery] string q, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new SearchPlacesQuery
        {
            SearchTerm = q,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// Authenticated users can submit a new place for discovery (goes to Pending queue)
    /// </summary>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreatePlaceCommand command)
    {
        command.SubmittedByUserId = _currentUserService.UserId;
        command.AutoApprove = _currentUserService.IsInRole("Admin") || _currentUserService.IsInRole("Moderator");

        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Update place (Moderator/Admin only)
    /// </summary>
    [Authorize(Policy = "ModeratorOnly")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePlaceCommand command)
    {
        if (id != command.Id) return BadRequest("Route id and body id mismatch.");

        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize]
    [HttpPost("upload-photos")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(List<string>), 200)]
    public async Task<ActionResult<List<string>>> UploadPlacePhotos(List<IFormFile> files)
    {
        var urls = await _mediator.Send(new UploadPlacePhotosCommand { Files = files });
        return Ok(urls);
    }

    /// <summary>
    /// Delete place (Admin only)
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeletePlaceCommand { Id = id });
        return NoContent();
    }

    /// <summary>
    /// Approve or reject a pending user-submitted place (Moderator/Admin)
    /// </summary>
    [Authorize(Policy = "ModeratorOnly")]
    [HttpPatch("{id:int}/approval")]
    public async Task<IActionResult> SetApproval(int id, [FromBody] ApprovePlaceCommand command)
    {
        if (id != command.PlaceId) return BadRequest("Route id and body id mismatch.");

        await _mediator.Send(command);
        return NoContent();
    }


   

/// <summary>
/// সব approved places — pagination + optional filters
/// </summary>
[HttpGet]
public async Task<ActionResult<PaginatedList<PlaceListItemDto>>> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 12,
    [FromQuery] int? divisionId = null,
    [FromQuery] int? districtId = null,
    [FromQuery] PlaceCategoryEnum? category = null,
    [FromQuery] string? search = null,
    [FromQuery] string sortBy = "rating")
{
    var result = await _mediator.Send(new GetAllPlacesQuery
    {
        PageNumber = pageNumber,
        PageSize = pageSize,
        DivisionId = divisionId,
        DistrictId = districtId,
        Category = category,
        SearchTerm = search,
        SortBy = sortBy
    });

    return Ok(result);
}
}