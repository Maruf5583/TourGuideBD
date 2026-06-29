using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.CreateBooking;

public class BookingResponseDto
{
    public int BookingId { get; set; }
    public string StripeClientSecret { get; set; } = string.Empty;
    public string PaymentIntentId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

public class CreateBookingCommand : IRequest<BookingResponseDto>
{
    public string UserId { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public int TourPackageId { get; set; }
    public DateTime TourDate { get; set; }
    public int NumberOfPeople { get; set; }
}

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.TourPackageId).GreaterThan(0);
        RuleFor(x => x.NumberOfPeople).InclusiveBetween(1, 50);
        RuleFor(x => x.TourDate).GreaterThan(DateTime.UtcNow.Date)
            .WithMessage("Tour date must be in the future.");
    }
}

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingResponseDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IStripeService _stripeService;
    private const decimal PlatformFeePercent = 0.10m; // 10%

    public CreateBookingCommandHandler(
        IApplicationDbContext context,
        IStripeService stripeService)
    {
        _context = context;
        _stripeService = stripeService;
    }

    public async Task<BookingResponseDto> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken)
    {
        var package = await _context.TourPackages
            .Include(t => t.GuideProfile)
            .Include(t => t.Availabilities)
            .FirstOrDefaultAsync(t => t.Id == request.TourPackageId && t.IsActive, cancellationToken);

        if (package == null)
        {
            throw new NotFoundException("TourPackage", request.TourPackageId);
        }

        // People count check
        if (request.NumberOfPeople > package.MaxPeople)
        {
            throw new DomainValidationException("NumberOfPeople",
                $"Maximum {package.MaxPeople} people allowed for this package.");
        }

        // Availability check
        var availability = package.Availabilities
            .FirstOrDefault(a => a.AvailableDate.Date == request.TourDate.Date);

        if (availability == null || !availability.IsAvailable)
        {
            throw new DomainValidationException("TourDate", "This date is not available.");
        }

        if (availability.CurrentBookings >= availability.MaxBookings)
        {
            throw new DomainValidationException("TourDate", "This date is fully booked.");
        }

        // Cost calculate
        var totalAmount = package.PricePerPerson * request.NumberOfPeople;
        var platformFee = Math.Round(totalAmount * PlatformFeePercent, 2);
        var guideEarning = totalAmount - platformFee;

        // Stripe PaymentIntent create
        var paymentResult = await _stripeService.CreatePaymentIntentAsync(
            totalAmount,
            "bdt",
            $"Tour booking: {package.Title}",
            request.UserEmail,
            new Dictionary<string, string>
            {
                { "packageId", package.Id.ToString() },
                { "userId", request.UserId },
                { "tourDate", request.TourDate.ToString("yyyy-MM-dd") }
            },
            cancellationToken);

        // Booking create
        var booking = new Booking
        {
            UserId = request.UserId,
            TourPackageId = request.TourPackageId,
            TourDate = request.TourDate,
            NumberOfPeople = request.NumberOfPeople,
            TotalAmount = totalAmount,
            PlatformFee = platformFee,
            GuideEarning = guideEarning,
            Status = BookingStatus.Pending,
            StripePaymentIntentId = paymentResult.PaymentIntentId,
            IsPaid = false,
            CreatedAt = DateTime.UtcNow
        };

        // Availability update
        availability.CurrentBookings += 1;
        if (availability.CurrentBookings >= availability.MaxBookings)
        {
            availability.IsAvailable = false;
        }

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);

        return new BookingResponseDto
        {
            BookingId = booking.Id,
            StripeClientSecret = paymentResult.ClientSecret,
            PaymentIntentId = paymentResult.PaymentIntentId,
            TotalAmount = totalAmount
        };
    }
}