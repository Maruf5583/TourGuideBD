using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Places.Queries.SearchPlaces;

public class SearchPlacesQuery : IRequest<PaginatedList<PlaceListItemDto>>
{
    public string SearchTerm { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class SearchPlacesQueryValidator : AbstractValidator<SearchPlacesQuery>
{
    public SearchPlacesQueryValidator()
    {
        RuleFor(x => x.SearchTerm).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class SearchPlacesQueryHandler : IRequestHandler<SearchPlacesQuery, PaginatedList<PlaceListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchPlacesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PlaceListItemDto>> Handle(SearchPlacesQuery request, CancellationToken cancellationToken)
    {
        var term = request.SearchTerm.Trim().ToLower();

        var query = _context.Places
            .Where(p => p.ApprovalStatus == ApprovalStatus.Approved &&
                (p.Name.ToLower().Contains(term)
                 || p.NameBn.Contains(term)
                 || p.District.Name.ToLower().Contains(term)
                 || p.Division.Name.ToLower().Contains(term)
                 || p.Tags.Any(t => t.Tag.ToLower().Contains(term))))
            .OrderByDescending(p => p.AverageRating)
            .ProjectTo<PlaceListItemDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<PlaceListItemDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}