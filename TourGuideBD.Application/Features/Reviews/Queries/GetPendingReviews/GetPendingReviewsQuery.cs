using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Reviews.Queries.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Reviews.Queries.GetPendingReviews;

public class GetPendingReviewsQuery : IRequest<PaginatedList<PendingReviewDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetPendingReviewsQueryValidator : AbstractValidator<GetPendingReviewsQuery>
{
    public GetPendingReviewsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetPendingReviewsQueryHandler : IRequestHandler<GetPendingReviewsQuery, PaginatedList<PendingReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetPendingReviewsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PendingReviewDto>> Handle(GetPendingReviewsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Reviews
            .Where(r => r.Status == ApprovalStatus.Pending)
            .OrderBy(r => r.CreatedAt)
            .ProjectTo<PendingReviewDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<PendingReviewDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}