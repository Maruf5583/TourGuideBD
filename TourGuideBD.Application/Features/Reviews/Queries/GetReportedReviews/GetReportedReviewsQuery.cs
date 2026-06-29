using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Reviews.Queries.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Reviews.Queries.GetReportedReviews;

public class GetReportedReviewsQuery : IRequest<PaginatedList<ReportedReviewDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetReportedReviewsQueryValidator : AbstractValidator<GetReportedReviewsQuery>
{
    public GetReportedReviewsQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetReportedReviewsQueryHandler : IRequestHandler<GetReportedReviewsQuery, PaginatedList<ReportedReviewDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetReportedReviewsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ReportedReviewDto>> Handle(GetReportedReviewsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.ReviewReports
            .Where(r => r.Status == ReportStatus.Open)
            .OrderBy(r => r.CreatedAt)
            .ProjectTo<ReportedReviewDto>(_mapper.ConfigurationProvider);

        return await PaginatedList<ReportedReviewDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}