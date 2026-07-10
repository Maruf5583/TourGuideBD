using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Guide.Commands.AddPaymentMethod;
using TourGuideBD.Application.Features.Guide.Commands.ApplyForGuide;
using TourGuideBD.Application.Features.Guide.Commands.CompleteTour;
using TourGuideBD.Application.Features.Guide.Commands.ConfirmBookingPayment;
using TourGuideBD.Application.Features.Guide.Commands.CreateBooking;
using TourGuideBD.Application.Features.Guide.Commands.CreateTourPackage;
using TourGuideBD.Application.Features.Guide.Commands.CreateWithdrawalRequest;
using TourGuideBD.Application.Features.Guide.Commands.DeletePaymentMethod;
using TourGuideBD.Application.Features.Guide.Commands.DeleteTourPackage;
using TourGuideBD.Application.Features.Guide.Commands.RemoveGuide;
using TourGuideBD.Application.Features.Guide.Commands.ReviewGuideApplication;
using TourGuideBD.Application.Features.Guide.Commands.SubmitGuideReview;
using TourGuideBD.Application.Features.Guide.Commands.UpdateLiveLocation;
using TourGuideBD.Application.Features.Guide.Commands.UpdateTourPackage;
using TourGuideBD.Application.Features.Guide.Queries.GetAllGuides;
using TourGuideBD.Application.Features.Guide.Queries.GetGuideBalance;
using TourGuideBD.Application.Features.Guide.Queries.GetGuideBookings;
using TourGuideBD.Application.Features.Guide.Queries.GetGuideById;
using TourGuideBD.Application.Features.Guide.Queries.GetGuidePackages;
using TourGuideBD.Application.Features.Guide.Queries.GetGuideRevenue;
using TourGuideBD.Application.Features.Guide.Queries.GetMyBookingById;
using TourGuideBD.Application.Features.Guide.Queries.GetMyBookings;
using TourGuideBD.Application.Features.Guide.Queries.GetPaymentMethods;
using TourGuideBD.Application.Features.Guide.Queries.GetPendingApplications;
using TourGuideBD.Application.Features.Guide.Queries.GetWithdrawalHistory;
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

    /// <summary>
    /// Client-side payment confirm (TESTING ONLY - production এ webhook use করবেন)
    /// </summary>
    [Authorize]
    [HttpPost("bookings/{bookingId:int}/confirm-payment")]
    public async Task<IActionResult> ConfirmPaymentClient(
        int bookingId, [FromBody] ConfirmPaymentClientRequest body)
    {
        await _mediator.Send(new ConfirmBookingPaymentCommand
        {
            PaymentIntentId = body.PaymentIntentId,
            ChargeId = body.ChargeId ?? string.Empty
        });
        return NoContent();
    }

    public class ConfirmPaymentClientRequest
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public string? ChargeId { get; set; }
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


    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{guideProfileId:int}/remove")]
    public async Task<IActionResult> RemoveGuide(
        int guideProfileId, [FromBody] RemoveGuideRequestBody body)
    {
        await _mediator.Send(new RemoveGuideCommand
        {
            GuideProfileId = guideProfileId,
            Reason = body.Reason,
            AdminUserId = _currentUserService.UserId!
        });

        return NoContent();
    }


    public class RemoveGuideRequestBody
    {
        public string Reason { get; set; } = string.Empty;
    }




    /// <summary>
    /// User — নিজের সব booking history দেখো (Profile page এ দেখানোর জন্য)
    /// </summary>
    [Authorize]
    [HttpGet("my-bookings")]
    public async Task<ActionResult<PaginatedList<MyBookingDto>>> GetMyBookings(
        [FromQuery] BookingStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetMyBookingsQuery
        {
            UserId = _currentUserService.UserId!,
            Status = status,
            PageNumber = pageNumber,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// User — একটা specific booking এর details দেখো
    /// </summary>
    [Authorize]
    [HttpGet("my-bookings/{bookingId:int}")]
    public async Task<ActionResult<MyBookingDto>> GetMyBookingById(int bookingId)
    {
        var result = await _mediator.Send(new GetMyBookingByIdQuery
        {
            BookingId = bookingId,
            UserId = _currentUserService.UserId!
        });

        return Ok(result);
    }
    // ========== GUIDE — BOOKING MANAGEMENT ==========

    /// <summary>
    /// Guide — নিজের সব bookings দেখো
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpGet("my-guide-bookings")]
    public async Task<ActionResult<PaginatedList<GuideBookingDto>>> GetGuideBookings(
        [FromQuery] BookingStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetGuideBookingsQuery
        {
            GuideUserId = _currentUserService.UserId!,
            Status = status,
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Ok(result);
    }

    /// <summary>
    /// Guide — Tour complete mark করো
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpPatch("bookings/{bookingId:int}/complete")]
    public async Task<IActionResult> CompleteTour(int bookingId)
    {
        await _mediator.Send(new CompleteTourCommand
        {
            BookingId = bookingId,
            GuideUserId = _currentUserService.UserId!
        });
        return NoContent();
    }

    // ========== USER — GUIDE REVIEW ==========

    /// <summary>
    /// User — Tour complete হলে review দাও (auto approved — no moderation)
    /// </summary>
    [Authorize]
    [HttpPost("bookings/{bookingId:int}/review")]
    public async Task<IActionResult> SubmitReview(
        int bookingId, [FromBody] SubmitGuideReviewCommand command)
    {
        if (bookingId != command.BookingId) return BadRequest("Id mismatch.");
        command.UserId = _currentUserService.UserId!;
        await _mediator.Send(command);
        return NoContent();
    }

    // ========== GUIDE — BALANCE & WITHDRAWAL ==========

    /// <summary>
    /// Guide — Available balance দেখো
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpGet("my-balance")]
    public async Task<ActionResult<GuideBalanceDto>> GetMyBalance()
    {
        var result = await _mediator.Send(new GetGuideBalanceQuery
        {
            GuideUserId = _currentUserService.UserId!
        });
        return Ok(result);
    }

    /// <summary>
    /// Guide — Payment method add করো (bKash/Nagad/Bank)
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpPost("payment-methods")]
    public async Task<ActionResult<int>> AddPaymentMethod(
        [FromBody] AddPaymentMethodCommand command)
    {
        command.GuideUserId = _currentUserService.UserId!;
        var id = await _mediator.Send(command);
        return Ok(new { paymentMethodId = id });
    }

    /// <summary>
    /// Guide — Withdrawal request করো (minimum 500 BDT)
    /// Processing fee: 1%
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpPost("withdrawal-request")]
    public async Task<ActionResult<int>> CreateWithdrawalRequest(
        [FromBody] CreateWithdrawalRequestCommand command)
    {
        command.GuideUserId = _currentUserService.UserId!;
        var id = await _mediator.Send(command);
        return Ok(new { withdrawalId = id });
    }
    /// <summary>
    /// Guide — নিজের সব Payment Methods list দেখো
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpGet("payment-methods")]
    public async Task<ActionResult<List<PaymentMethodDto>>> GetPaymentMethods()
    {
        var query = new GetPaymentMethodsQuery
        {
            GuideUserId = _currentUserService.UserId!
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }




    /// <summary>
    /// Guide — Payment method delete করো
    /// Pending withdrawal এ use হলে delete হবে না
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpDelete("payment-methods/{paymentMethodId:int}")]
    public async Task<IActionResult> DeletePaymentMethod(int paymentMethodId)
    {
        await _mediator.Send(new DeletePaymentMethodCommand
        {
            PaymentMethodId = paymentMethodId,
            GuideUserId = _currentUserService.UserId!
        });
        return NoContent();
    }


    /// <summary>
    /// Guide — Withdrawal history দেখো
    /// </summary>
    [Authorize(Policy = "TourGuideOnly")]
    [HttpGet("withdrawal-history")]
    public async Task<ActionResult<PaginatedList<WithdrawalHistoryDto>>> GetWithdrawalHistory(
        [FromQuery] WithdrawalStatus? status,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetWithdrawalHistoryQuery
        {
            GuideUserId = _currentUserService.UserId!,
            Status = status,
            PageNumber = pageNumber,
            PageSize = pageSize
        });
        return Ok(result);
    }
}