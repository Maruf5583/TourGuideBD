using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Guide.Commands.ApplyForGuide;
using TourGuideBD.Application.Features.Guide.Commands.ConfirmBookingPayment;
using TourGuideBD.Application.Features.Guide.Commands.CreateBooking;
using TourGuideBD.Application.Features.Guide.Commands.CreateTourPackage;
using TourGuideBD.Application.Features.Guide.Commands.DeleteTourPackage;
using TourGuideBD.Application.Features.Guide.Commands.ReviewGuideApplication;
using TourGuideBD.Application.Features.Guide.Commands.UpdateLiveLocation;
using TourGuideBD.Application.Features.Guide.Commands.UpdateTourPackage;
using TourGuideBD.Application.Features.Guide.Queries.GetAllGuides;
using TourGuideBD.Application.Features.Guide.Queries.GetGuideById;
using TourGuideBD.Application.Features.Guide.Queries.GetGuidePackages;
using TourGuideBD.Application.Features.Guide.Queries.GetGuideRevenue;
using TourGuideBD.Application.Features.Guide.Queries.GetPendingApplications;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Api.Controllers.v1;

[ApiController]
[Route("api/v1/guide")]
public class GuideController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    private readonly IConfiguration _configuration;

    public GuideController(
        IMediator mediator,
        ICurrentUserService currentUserService,
        IConfiguration configuration)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _configuration = configuration;
    }

    // ========== PHASE 1 — Application ==========

    /// <summary>
    /// Guide হওয়ার জন্য apply করো
    /// Documents: NID Front/Back Photo + DOB Certificate Photo upload করে URL দাও
    /// </summary>
    [Authorize]
    [HttpPost("apply")]
    public async Task<ActionResult<int>> Apply([FromBody] ApplyForGuideCommand command)
    {
        command.UserId = _currentUserService.UserId!;
        var id = await _mediator.Send(command);
        return Ok(new
        {
            applicationId = id,
            message = "Application submitted successfully. Admin will review within 3-5 business days."
        });
    }

    /// <summary>
    /// Document upload করো (NID/DOB Certificate) — URL return করবে
    /// </summary>
    [Authorize]
    [HttpPost("upload-documents")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(object), 200)]
    public async Task<ActionResult> UploadDocuments(
        [FromServices] IBlobStorageService blobStorageService,
        IFormFile file,
        [FromQuery] string docType)
    {
        var allowedTypes = new[] { "nid-front", "nid-back", "dob-certificate", "profile-photo", "certificate" };

        if (!allowedTypes.Contains(docType))
        {
            return BadRequest("Invalid document type.");
        }

        using var stream = file.OpenReadStream();
        var url = await blobStorageService.UploadAsync(
            stream, file.FileName, file.ContentType,
            $"guide-documents/{docType}");

        return Ok(new { url, docType });
    }

    // ========== ADMIN — Application Management ==========

    /// <summary>
    /// Admin — Pending guide applications দেখো
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpGet("applications")]
    public async Task<ActionResult<PaginatedList<GuideApplicationDto>>> GetApplications(
        [FromQuery] GuideApplicationStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetPendingApplicationsQuery
        {
            Status = status,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// Admin — Guide application approve/reject করো
    /// </summary>
    [Authorize(Policy = "AdminOnly")]
    [HttpPatch("applications/{id:int}/review")]
    public async Task<IActionResult> ReviewApplication(
        int id, [FromBody] ReviewGuideApplicationCommand command)
    {
        if (id != command.ApplicationId) return BadRequest("Id mismatch.");
        command.AdminUserId = _currentUserService.UserId!;
        await _mediator.Send(command);
        return NoContent();
    }

    // ========== PHASE 2 — Tour Package ==========

    /// <summary>
    /// Guide — Tour package create করো
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpPost("packages")]
    public async Task<ActionResult<int>> CreatePackage([FromBody] CreateTourPackageCommand command)
    {
        command.GuideUserId = _currentUserService.UserId!;
        var id = await _mediator.Send(command);
        return Ok(new { packageId = id });
    }

    // ========== BOOKING ==========

    /// <summary>
    /// User — Tour book করো
    /// Response এ Stripe ClientSecret পাবে — Frontend দিয়ে payment complete করো
    /// </summary>
    [Authorize]
    [HttpPost("bookings")]
    public async Task<ActionResult<BookingResponseDto>> CreateBooking(
        [FromBody] CreateBookingCommand command)
    {
        command.UserId = _currentUserService.UserId!;
        command.UserEmail = _currentUserService.Email!;
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    // ========== LIVE LOCATION ==========

    /// <summary>
    /// Guide — Live location update করো (tour চলাকালীন)
    /// User real-time এ guide এর location দেখতে পাবে (SignalR)
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpPost("location/update")]
    public async Task<IActionResult> UpdateLocation([FromBody] UpdateLiveLocationCommand command)
    {
        command.GuideUserId = _currentUserService.UserId!;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Guide — Location sharing বন্ধ করো
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpPost("location/stop/{bookingId:int}")]
    public async Task<IActionResult> StopLocationSharing(int bookingId)
    {
        await _mediator.Send(new UpdateLiveLocationCommand
        {
            GuideUserId = _currentUserService.UserId!,
            BookingId = bookingId,
            Latitude = 0,
            Longitude = 0,
            IsSharing = false
        });
        return NoContent();
    }

    // ========== REVENUE ANALYTICS ==========

    /// <summary>
    /// Guide — নিজের revenue/earnings দেখো
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpGet("revenue")]
    public async Task<ActionResult<GuideRevenueDto>> GetRevenue([FromQuery] int? year)
    {
        var result = await _mediator.Send(new GetGuideRevenueQuery
        {
            GuideUserId = _currentUserService.UserId!,
            Year = year
        });
        return Ok(result);
    }

    // ========== STRIPE WEBHOOK ==========

    /// <summary>
    /// Stripe Webhook — Payment success হলে booking confirm হবে
    /// </summary>
    [HttpPost("webhook/stripe")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var webhookSecret = _configuration["Stripe:WebhookSecret"]!;

        try
        {
            var stripeEvent = Stripe.EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                webhookSecret);

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as Stripe.PaymentIntent;
                if (paymentIntent != null)
                {
                    await _mediator.Send(new ConfirmBookingPaymentCommand
                    {
                        PaymentIntentId = paymentIntent.Id,
                        ChargeId = paymentIntent.LatestChargeId ?? string.Empty
                    });
                }
            }

            return Ok();
        }
        catch (Stripe.StripeException)
        {
            return BadRequest("Invalid webhook signature.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedList<GuideListItemDto>>> GetAllGuides(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 12,
    [FromQuery] string? search = null,
    [FromQuery] int? districtId = null,
    [FromQuery] string sortBy = "rating")
    {
        var result = await _mediator.Send(new GetAllGuidesQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            SearchTerm = search,
            DistrictId = districtId,
            SortBy = sortBy
        });
        return Ok(result);
    }

    /// <summary>
    /// Guide এর profile + সব packages দেখো (User/Public)
    /// </summary>
    [HttpGet("{guideId:int}")]
    public async Task<ActionResult<GuideDetailDto>> GetGuideById(int guideId)
    {
        var result = await _mediator.Send(new GetGuideByIdQuery { GuideId = guideId });
        return Ok(result);
    }

    /// <summary>
    /// Guide নিজের সব packages দেখবে
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpGet("my-packages")]
    public async Task<ActionResult<List<GuidePackageSummaryDto>>> GetMyPackages()
    {
        var result = await _mediator.Send(new GetGuidePackagesQuery
        {
            GuideUserId = _currentUserService.UserId!
        });
        return Ok(result);
    }


    /// <summary>
    /// Guide — Package edit করো
    /// Active booking থাকলে price change করা যাবে না
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpPut("packages/{packageId:int}")]
    public async Task<IActionResult> UpdatePackage(
        int packageId, [FromBody] UpdateTourPackageCommand command)
    {
        if (packageId != command.PackageId) return BadRequest("Id mismatch.");
        command.GuideUserId = _currentUserService.UserId!;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Guide — Package deactivate/delete করো
    /// Active booking থাকলে delete করা যাবে না
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpDelete("packages/{packageId:int}")]
    public async Task<IActionResult> DeletePackage(int packageId)
    {
        await _mediator.Send(new DeleteTourPackageCommand
        {
            PackageId = packageId,
            GuideUserId = _currentUserService.UserId!
        });
        return NoContent();
    }




}