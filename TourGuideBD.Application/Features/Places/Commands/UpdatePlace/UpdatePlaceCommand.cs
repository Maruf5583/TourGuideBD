using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Behaviours;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.PlaceDetails;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Places.Commands.UpdatePlace;

public class UpdatePlaceCommand : IRequest<Unit>, IAuditableRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public decimal EntryFee { get; set; }
    public BestSeason BestSeason { get; set; } = BestSeason.AllYear;

    public int DivisionId { get; set; }
    public int DistrictId { get; set; }
    public int? UpazilaId { get; set; }

    public List<int> CategoryIds { get; set; } = new();
    public List<string> Tags { get; set; } = new();

    // IAuditableRequest
    public string ActionName => "UpdatePlace";
    public string EntityName => nameof(Place);
    public string? EntityId => Id.ToString();
}

public class UpdatePlaceCommandValidator : AbstractValidator<UpdatePlaceCommand>
{
    public UpdatePlaceCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
        RuleFor(x => x.NameBn).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(4000);

        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);

        RuleFor(x => x.EntryFee).GreaterThanOrEqualTo(0);

        RuleFor(x => x.DivisionId).GreaterThan(0);
        RuleFor(x => x.DistrictId).GreaterThan(0);

        RuleFor(x => x.CategoryIds).NotEmpty();
    }
}

public class UpdatePlaceCommandHandler : IRequestHandler<UpdatePlaceCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public UpdatePlaceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdatePlaceCommand request, CancellationToken cancellationToken)
    {
        var place = await _context.Places
            .Include(p => p.CategoryMaps)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (place == null)
        {
            throw new NotFoundException(nameof(Place), request.Id);
        }

        var divisionExists = await _context.Divisions.AnyAsync(d => d.Id == request.DivisionId, cancellationToken);
        if (!divisionExists) throw new NotFoundException(nameof(Division), request.DivisionId);

        var districtExists = await _context.Districts.AnyAsync(d => d.Id == request.DistrictId && d.DivisionId == request.DivisionId, cancellationToken);
        if (!districtExists) throw new NotFoundException(nameof(District), request.DistrictId);

        if (request.UpazilaId.HasValue)
        {
            var upazilaExists = await _context.Upazilas.AnyAsync(u => u.Id == request.UpazilaId.Value && u.DistrictId == request.DistrictId, cancellationToken);
            if (!upazilaExists) throw new NotFoundException("Upazila", request.UpazilaId.Value);
        }

        var categories = await _context.PlaceCategories
            .Where(c => request.CategoryIds.Contains(c.Id))
            .ToListAsync(cancellationToken);

        if (categories.Count != request.CategoryIds.Distinct().Count())
        {
            throw new NotFoundException("One or more PlaceCategory ids are invalid.", string.Join(",", request.CategoryIds));
        }

        place.Name = request.Name;
        place.NameBn = request.NameBn;
        place.Description = request.Description;
        place.Latitude = request.Latitude;
        place.Longitude = request.Longitude;
        place.EntryFee = request.EntryFee;
        place.BestSeason = request.BestSeason;
        place.DivisionId = request.DivisionId;
        place.DistrictId = request.DistrictId;
        place.UpazilaId = request.UpazilaId;
        place.UpdatedAt = DateTime.UtcNow;

        // Replace category maps
        _context.PlaceCategoryMaps.RemoveRange(place.CategoryMaps);
        place.CategoryMaps.Clear();
        foreach (var category in categories)
        {
            place.CategoryMaps.Add(new PlaceCategoryMap { PlaceId = place.Id, PlaceCategoryId = category.Id });
        }

        // Replace tags
        _context.PlaceTags.RemoveRange(place.Tags);
        place.Tags.Clear();
        foreach (var tag in request.Tags.Distinct())
        {
            place.Tags.Add(new PlaceTag { PlaceId = place.Id, Tag = tag });
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}