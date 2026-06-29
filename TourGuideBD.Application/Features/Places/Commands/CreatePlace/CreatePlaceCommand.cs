using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.PlaceDetails;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Places.Commands.CreatePlace;

public class CreatePlaceCommand : IRequest<int>, IAuditableRequest
{
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public decimal EntryFee { get; set; }
    public BestSeason BestSeason { get; set; } = BestSeason.AllYear;

    public string? OpeningHours { get; set; }
    public string? ClosingHours { get; set; }

    public int DivisionId { get; set; }
    public int DistrictId { get; set; }
    public int? UpazilaId { get; set; }

    public List<int> CategoryIds { get; set; } = new();
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Photo URLs — already uploaded via /api/v1/places/upload-photos
    /// Maximum 5 photos
    /// </summary>
    public List<string> PhotoUrls { get; set; } = new();

    public string? SubmittedByUserId { get; set; }
    public bool AutoApprove { get; set; } = false;

    public string ActionName => "CreatePlace";
    public string EntityName => nameof(Place);
    public string? EntityId { get; set; }
}

public class CreatePlaceCommandValidator : AbstractValidator<CreatePlaceCommand>
{
    public CreatePlaceCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.NameBn).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(4000);

        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);

        RuleFor(x => x.EntryFee).GreaterThanOrEqualTo(0);

        RuleFor(x => x.DivisionId).GreaterThan(0);
        RuleFor(x => x.DistrictId).GreaterThan(0);

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage("At least one category is required.");

        RuleFor(x => x.OpeningHours).MaximumLength(100);
        RuleFor(x => x.ClosingHours).MaximumLength(100);

        RuleForEach(x => x.Tags).MaximumLength(50);

        RuleFor(x => x.PhotoUrls)
            .Must(p => p.Count <= 5)
            .WithMessage("Maximum 5 photos allowed.");
    }
}

public class CreatePlaceCommandHandler : IRequestHandler<CreatePlaceCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreatePlaceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreatePlaceCommand request, CancellationToken cancellationToken)
    {
        var divisionExists = await _context.Divisions
            .AnyAsync(d => d.Id == request.DivisionId, cancellationToken);
        if (!divisionExists)
            throw new NotFoundException(nameof(Division), request.DivisionId);

        var districtExists = await _context.Districts
            .AnyAsync(d => d.Id == request.DistrictId
                && d.DivisionId == request.DivisionId, cancellationToken);
        if (!districtExists)
            throw new NotFoundException(nameof(District), request.DistrictId);

        if (request.UpazilaId.HasValue)
        {
            var upazilaExists = await _context.Upazilas
                .AnyAsync(u => u.Id == request.UpazilaId.Value
                    && u.DistrictId == request.DistrictId, cancellationToken);
            if (!upazilaExists)
                throw new NotFoundException("Upazila", request.UpazilaId.Value);
        }

        var categories = await _context.PlaceCategories
            .Where(c => request.CategoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        if (categories.Count != request.CategoryIds.Distinct().Count())
        {
            throw new NotFoundException(
                "One or more PlaceCategory ids are invalid.",
                string.Join(",", request.CategoryIds));
        }

        var place = new Place
        {
            Name = request.Name,
            NameBn = request.NameBn,
            Description = request.Description,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            EntryFee = request.EntryFee,
            BestSeason = request.BestSeason,
            OpeningHours = request.OpeningHours,
            ClosingHours = request.ClosingHours,
            DivisionId = request.DivisionId,
            DistrictId = request.DistrictId,
            UpazilaId = request.UpazilaId,
            SubmittedByUserId = request.SubmittedByUserId,
            ApprovalStatus = request.AutoApprove
                ? ApprovalStatus.Approved
                : ApprovalStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var category in categories)
        {
            place.CategoryMaps.Add(new PlaceCategoryMap
            {
                PlaceCategory = category
            });
        }

        foreach (var tag in request.Tags.Distinct())
        {
            place.Tags.Add(new PlaceTag { Tag = tag });
        }

        // Maximum 5 photos
        var isFirst = true;
        foreach (var photoUrl in request.PhotoUrls.Take(5))
        {
            place.Photos.Add(new PlacePhoto
            {
                Url = photoUrl,
                IsCover = isFirst,
                UploadedAt = DateTime.UtcNow
            });
            isFirst = false;
        }

        _context.Places.Add(place);
        await _context.SaveChangesAsync(cancellationToken);

        request.EntityId = place.Id.ToString();

        return place.Id;
    }
}