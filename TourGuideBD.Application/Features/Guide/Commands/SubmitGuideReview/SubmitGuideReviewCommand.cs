using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using DomainValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Guide.Commands.SubmitGuideReview;

public class SubmitGuideReviewCommand : IRequest<Unit>
{
    public string UserId { get; set; } = string.Empty;
    public int BookingId { get; set; }

    // Ratings 1-5
    public int PunctualityRating { get; set; }
    public int KnowledgeRating { get; set; }
    public int CommunicationRating { get; set; }
    public int SafetyRating { get; set; }
    public int ValueRating { get; set; }

    public string? Comment { get; set; }
}

public class SubmitGuideReviewCommandValidator : AbstractValidator<SubmitGuideReviewCommand>
{
    public SubmitGuideReviewCommandValidator()
    {
        RuleFor(x => x.BookingId).GreaterThan(0);
        RuleFor(x => x.PunctualityRating).InclusiveBetween(1, 5);
        RuleFor(x => x.KnowledgeRating).InclusiveBetween(1, 5);
        RuleFor(x => x.CommunicationRating).InclusiveBetween(1, 5);
        RuleFor(x => x.SafetyRating).InclusiveBetween(1, 5);
        RuleFor(x => x.ValueRating).InclusiveBetween(1, 5);
        RuleFor(x => x.Comment).MaximumLength(500);
    }
}

public class SubmitGuideReviewCommandHandler : IRequestHandler<SubmitGuideReviewCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public SubmitGuideReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        SubmitGuideReviewCommand request,
        CancellationToken cancellationToken)
    {
        var booking = await _context.Bookings
            .Include(b => b.TourPackage)
                .ThenInclude(p => p.GuideProfile)
            .Include(b => b.Review)
            .FirstOrDefaultAsync(b =>
                b.Id == request.BookingId && b.UserId == request.UserId,
                cancellationToken);

        if (booking == null)
            throw new NotFoundException("Booking", request.BookingId);

        if (booking.Status != BookingStatus.Completed)
            throw new DomainValidationException("BookingId",
                "You can only review completed tours.");

        if (booking.Review != null)
            throw new DomainValidationException("BookingId",
                "You have already reviewed this tour.");

        var overallRating = (int)Math.Round(
            (request.PunctualityRating +
             request.KnowledgeRating +
             request.CommunicationRating +
             request.SafetyRating +
             request.ValueRating) / 5.0);

        // Review auto-approved — no moderation needed
        var review = new GuideReview
        {
            UserId = request.UserId,
            GuideProfileId = booking.TourPackage.GuideProfileId,
            BookingId = request.BookingId,
            PunctualityRating = request.PunctualityRating,
            KnowledgeRating = request.KnowledgeRating,
            CommunicationRating = request.CommunicationRating,
            SafetyRating = request.SafetyRating,
            ValueRating = request.ValueRating,
            OverallRating = overallRating,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        _context.GuideReviews.Add(review);

        // Guide rating recalculate করো
        var guide = booking.TourPackage.GuideProfile;
        guide.TotalReviews += 1;

        var allReviews = await _context.GuideReviews
            .Where(r => r.GuideProfileId == guide.Id)
            .Select(r => r.OverallRating)
            .ToListAsync(cancellationToken);

        allReviews.Add(overallRating);
        guide.AverageRating = Math.Round(allReviews.Average(), 1);

        // Badge update করো
        guide.Badge = guide.AverageRating switch
        {
            >= 4.8 when guide.TotalToursCompleted >= 100 => GuideBadge.Expert,
            >= 4.5 when guide.TotalToursCompleted >= 50 => GuideBadge.TopRated,
            >= 4.0 => GuideBadge.Certified,
            _ => GuideBadge.Verified
        };

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}