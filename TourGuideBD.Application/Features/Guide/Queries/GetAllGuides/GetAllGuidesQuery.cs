using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;

namespace TourGuideBD.Application.Features.Guide.Queries.GetAllGuides;

public class GuideListItemDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string ProfilePhotoUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public string Specialities { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public int TotalToursCompleted { get; set; }
    public string Badge { get; set; } = string.Empty;
    public int TotalPackages { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}

public class GetAllGuidesQuery : IRequest<PaginatedList<GuideListItemDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 12;
    public string? SearchTerm { get; set; }
    public int? DistrictId { get; set; }
    public string SortBy { get; set; } = "rating";
}

public class GetAllGuidesQueryValidator : AbstractValidator<GetAllGuidesQuery>
{
    public GetAllGuidesQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetAllGuidesQueryHandler : IRequestHandler<GetAllGuidesQuery, PaginatedList<GuideListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllGuidesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GuideListItemDto>> Handle(
        GetAllGuidesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.GuideProfiles
            .Where(g => g.IsActive);

        // Search by name or speciality
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(g =>
                g.FullName.ToLower().Contains(term) ||
                g.Specialities.ToLower().Contains(term) ||
                g.Languages.ToLower().Contains(term));
        }

        // Filter by district
        if (request.DistrictId.HasValue)
        {
            var districtId = request.DistrictId.Value.ToString();
            query = query.Where(g =>
                g.OperatingDistrictIds.Contains(districtId));
        }

        // Sort
        query = request.SortBy switch
        {
            "name" => query.OrderBy(g => g.FullName),
            "tours" => query.OrderByDescending(g => g.TotalToursCompleted),
            _ => query.OrderByDescending(g => g.AverageRating)
        };

        var projected = query.Select(g => new GuideListItemDto
        {
            Id = g.Id,
            UserId = g.UserId,
            FullName = g.FullName,
            ProfilePhotoUrl = g.ProfilePhotoUrl,
            PhoneNumber = g.PhoneNumber,
            Bio = g.Bio,
            Languages = g.Languages,
            Specialities = g.Specialities,
            ExperienceYears = g.ExperienceYears,
            AverageRating = g.AverageRating,
            TotalReviews = g.TotalReviews,
            TotalToursCompleted = g.TotalToursCompleted,
            Badge = g.Badge.ToString(),
            TotalPackages = g.TourPackages.Count(p => p.IsActive)
        });

        return await PaginatedList<GuideListItemDto>.CreateAsync(
            projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}