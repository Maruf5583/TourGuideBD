using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Reviews.Queries.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Reviews.Queries.GetReviewsByPlace;

public class GetReviewsByPlaceQuery : IRequest<PaginatedList<ReviewDto>>
{
    public int PlaceId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetReviewsByPlaceQueryValidator : AbstractValidator<GetReviewsByPlaceQuery>
{
    public GetReviewsByPlaceQueryValidator()
    {
        RuleFor(x => x.PlaceId).GreaterThan(0);
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetReviewsByPlaceQueryHandler : IRequestHandler<GetReviewsByPlaceQuery, PaginatedList<ReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReviewsByPlaceQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReviewDto>> Handle(GetReviewsByPlaceQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reviews
            .Where(r => r.PlaceId == request.PlaceId && r.Status == ApprovalStatus.Approved)
            .OrderByDescending(r => r.CreatedAt)
            .ProjectTo<ReviewDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<ReviewDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}