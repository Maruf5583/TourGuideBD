using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Users.Common;

namespace TourGuideBD.Application.Features.Users.Queries.GetVisitHistory;

public class GetVisitHistoryQuery : IRequest<PaginatedList<VisitHistoryDto>>
{
    public string UserId { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetVisitHistoryQueryValidator : AbstractValidator<GetVisitHistoryQuery>
{
    public GetVisitHistoryQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetVisitHistoryQueryHandler : IRequestHandler<GetVisitHistoryQuery, PaginatedList<VisitHistoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetVisitHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<VisitHistoryDto>> Handle(GetVisitHistoryQuery request, CancellationToken cancellationToken)
    {
        var query = _context.VisitHistories
            .Where(v => v.UserId == request.UserId)
            .OrderByDescending(v => v.VisitedAt)
            .Select(v => new VisitHistoryDto
            {
                Id = v.Id,
                PlaceId = v.PlaceId,
                PlaceName = v.Place.Name,
                PlaceCoverPhotoUrl = v.Place.Photos
                    .Where(p => p.IsCover)
                    .Select(p => p.Url)
                    .FirstOrDefault(),
                VisitedAt = v.VisitedAt
            });

        return await PaginatedList<VisitHistoryDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}