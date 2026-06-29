         using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Admin.Queries.GetPendingPlaces;

public class GetPendingPlacesQuery : IRequest<PaginatedList<PlaceListItemDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetPendingPlacesQueryValidator : AbstractValidator<GetPendingPlacesQuery>
{
    public GetPendingPlacesQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetPendingPlacesQueryHandler : IRequestHandler<GetPendingPlacesQuery, PaginatedList<PlaceListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPendingPlacesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PlaceListItemDto>> Handle(GetPendingPlacesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Places
            .Where(p => p.ApprovalStatus == ApprovalStatus.Pending)
            .OrderBy(p => p.CreatedAt)
            .ProjectTo<PlaceListItemDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<PlaceListItemDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}