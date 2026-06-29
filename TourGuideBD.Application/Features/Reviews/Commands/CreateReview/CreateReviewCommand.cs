using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.Reviews;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;
using ValidationException = TourGuideBD.Domain.Exceptions.ValidationException;

namespace TourGuideBD.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommand : IRequest<int>, IAuditableRequest
{
    public int PlaceId { get; set; }
    public int Rating { get; set; }

    public string? CommentEn { get; set; }
    public string? CommentBn { get; set; }

    /// <summary>
    /// Photo URLs already uploaded to Azure Blob (max 5)
    /// </summary>
    public List<string> PhotoUrls { get; set; } = new();

    /// <summary>
    /// Set internally from ICurrentUserService
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    public string ActionName => "CreateReview";
    public string EntityName => nameof(Review);
    public string? EntityId { get; set; }
}

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.PlaceId).GreaterThan(0);

        RuleFor(x => x.Rating).InclusiveBetween(1, 5);

        RuleFor(x => x.CommentEn).MaximumLength(500);
        RuleFor(x => x.CommentBn).MaximumLength(500);

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.CommentEn) || !string.IsNullOrWhiteSpace(x.CommentBn))
            .WithMessage("Provide a review comment in English or Bangla.");

        RuleFor(x => x.PhotoUrls)
            .Must(p => p.Count <= 5)
            .WithMessage("A maximum of 5 photos can be uploaded per review.");
    }
}

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateReviewCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var placeExists = await _context.Places
            .AnyAsync(p => p.Id == request.PlaceId && p.ApprovalStatus == ApprovalStatus.Approved, cancellationToken);

        if (!placeExists)
        {
            throw new NotFoundException(nameof(Place), request.PlaceId);
        }

        var alreadyReviewed = await _context.Reviews
            .AnyAsync(r => r.PlaceId == request.PlaceId && r.UserId == request.UserId, cancellationToken);

        if (alreadyReviewed)
        {
            throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
            {
                new(nameof(request.PlaceId), "You have already reviewed this place.")
            });
        }

        var review = new Review
        {
            PlaceId = request.PlaceId,
            UserId = request.UserId,
            Rating = request.Rating,
            CommentEn = request.CommentEn,
            CommentBn = request.CommentBn,
            Status = ApprovalStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var url in request.PhotoUrls.Take(5))
        {
            review.Photos.Add(new ReviewPhoto { Url = url });
        }

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync(cancellationToken);

        request.EntityId = review.Id.ToString();

        return review.Id;
    }
}