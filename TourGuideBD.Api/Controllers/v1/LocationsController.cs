using MediatR;
using Microsoft.AspNetCore.Mvc;
using TourGuideBD.Application.Features.Locations.Queries.GetDistrictsByDivision;
using TourGuideBD.Application.Features.Locations.Queries.GetDivisions;
using TourGuideBD.Application.Features.Locations.Queries.GetUpazilasByDistrict;

namespace TourGuideBD.Api.Controllers.v1;

[ApiController]
[Route("api/v1/locations")]
public class LocationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LocationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all 8 divisions
    /// </summary>
    [HttpGet("divisions")]
    public async Task<ActionResult<List<DivisionDto>>> GetDivisions()
    {
        var result = await _mediator.Send(new GetDivisionsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Get districts under a division
    /// </summary>
    [HttpGet("divisions/{divisionId:int}/districts")]
    public async Task<ActionResult<List<DistrictDto>>> GetDistricts(int divisionId)
    {
        var result = await _mediator.Send(new GetDistrictsByDivisionQuery { DivisionId = divisionId });
        return Ok(result);
    }

    /// <summary>
    /// Get upazilas under a district
    /// </summary>
    [HttpGet("districts/{districtId:int}/upazilas")]
    public async Task<ActionResult<List<UpazilaDto>>> GetUpazilas(int districtId)
    {
        var result = await _mediator.Send(new GetUpazilasByDistrictQuery { DistrictId = districtId });
        return Ok(result);
    }
}