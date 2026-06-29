using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Places.Queries.GetAllPlaces;

public class GetAllPlacesQuery : IRequest<PaginatedList<PlaceListItemDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 12;

    // Optional filters
    public int? DistrictId { get; set; }
    public int? DivisionId { get; set; }
    public PlaceCategoryEnum? Category { get; set; }
    public string? SearchTerm { get; set; }

    // Sorting
    public string SortBy { get; set; } = "rating"; // rating, name, newest
}

public class GetAllPlacesQueryValidator : AbstractValidator<GetAllPlacesQuery>
{
    public GetAllPlacesQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetAllPlacesQueryHandler : IRequestHandler<GetAllPlacesQuery, PaginatedList<PlaceListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllPlacesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PlaceListItemDto>> Handle(
        GetAllPlacesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Places
            .Where(p => p.ApprovalStatus == ApprovalStatus.Approved);

        // Filter by Division
        if (request.DivisionId.HasValue)
        {
            query = query.Where(p => p.DivisionId == request.DivisionId.Value);
        }

        // Filter by District
        if (request.DistrictId.HasValue)
        {
            query = query.Where(p => p.DistrictId == request.DistrictId.Value);
        }

        // Filter by Category
        if (request.Category.HasValue)
        {
            query = query.Where(p =>
                p.CategoryMaps.Any(cm =>
                    cm.PlaceCategory.CategoryType == request.Category.Value));
        }

        // Search by name/tag/district
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.Trim().ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.NameBn.Contains(term) ||
                p.District.Name.ToLower().Contains(term) ||
                p.Division.Name.ToLower().Contains(term) ||
                p.Tags.Any(t => t.Tag.ToLower().Contains(term)));
        }

        // Sorting
        query = request.SortBy switch
        {
            "name" => query.OrderBy(p => p.Name),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.AverageRating)
                             .ThenByDescending(p => p.TotalReviews)
        };

        var projected = query
            .ProjectTo<PlaceListItemDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<PlaceListItemDto>.CreateAsync(
            projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}