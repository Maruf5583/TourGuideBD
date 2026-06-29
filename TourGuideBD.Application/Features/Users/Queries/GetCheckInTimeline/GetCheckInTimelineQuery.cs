using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Application.Features.Users.Common;

namespace TourGuideBD.Application.Features.Users.Queries.GetCheckInTimeline;

public class GetCheckInTimelineQuery : IRequest<PaginatedList<CheckInDto>>
{
    public string UserId { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetCheckInTimelineQueryValidator : AbstractValidator<GetCheckInTimelineQuery>
{
    public GetCheckInTimelineQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
    }
}

public class GetCheckInTimelineQueryHandler : IRequestHandler<GetCheckInTimelineQuery, PaginatedList<CheckInDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCheckInTimelineQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CheckInDto>> Handle(GetCheckInTimelineQuery request, CancellationToken cancellationToken)
    {
        var query = _context.CheckIns
            .Where(c => c.UserId == request.UserId)
            .OrderByDescending(c => c.CheckedInAt)
            .Select(c => new CheckInDto
            {
                Id = c.Id,
                PlaceId = c.PlaceId,
                PlaceName = c.Place.Name,
                PlaceCoverPhotoUrl = c.Place.Photos
                    .Where(p => p.IsCover)
                    .Select(p => p.Url)
                    .FirstOrDefault(),
                Note = c.Note,
                CheckedInAt = c.CheckedInAt
            });

        return await PaginatedList<CheckInDto>.CreateAsync(query, request.PageNumber, request.PageSize, cancellationToken);
    }
}