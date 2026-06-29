using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Places.Queries.GetPlacesByDistrict;

public class GetPlacesByDistrictQuery : IRequest<PaginatedList<PlaceListItemDto>>
{
    public int DistrictId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetPlacesByDistrictQueryValidator : AbstractValidator<GetPlacesByDistrictQuery>
{
    public GetPlacesByDistrictQueryValidator()
    {
        RuleFor(x => x.DistrictId).GreaterThan(0);
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetPlacesByDistrictQueryHandler : IRequestHandler<GetPlacesByDistrictQuery, PaginatedList<PlaceListItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPlacesByDistrictQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PlaceListItemDto>> Handle(GetPlacesByDistrictQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Places
            .Where(p => p.DistrictId == request.DistrictId && p.ApprovalStatus == ApprovalStatus.Approved)
            .OrderByDescending(p => p.AverageRating)
            .ProjectTo<PlaceListItemDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<PlaceListItemDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}