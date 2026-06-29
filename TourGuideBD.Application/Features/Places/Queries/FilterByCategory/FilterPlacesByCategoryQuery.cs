using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Places.Queries.FilterByCategory;

public class FilterPlacesByCategoryQuery : IRequest<PaginatedList<PlaceListItemDto>>
{
    public PlaceCategoryEnum Category { get; set; }
    public int? DistrictId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class FilterPlacesByCategoryQueryValidator : AbstractValidator<FilterPlacesByCategoryQuery>
{
    public FilterPlacesByCategoryQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class FilterPlacesByCategoryQueryHandler : IRequestHandler<FilterPlacesByCategoryQuery, PaginatedList<PlaceListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public FilterPlacesByCategoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PlaceListItemDto>> Handle(FilterPlacesByCategoryQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Places
            .Where(p => p.ApprovalStatus == ApprovalStatus.Approved
                && p.CategoryMaps.Any(cm => cm.PlaceCategory.CategoryType == request.Category));

        if (request.DistrictId.HasValue)
        {
            query = query.Where(p => p.DistrictId == request.DistrictId.Value);
        }

        var projected = query
            .OrderByDescending(p => p.AverageRating)
            .ProjectTo<PlaceListItemDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<PlaceListItemDto>.CreateAsync(projected, request.PageNumber, request.PageSize, cancellationToken);
    }
}