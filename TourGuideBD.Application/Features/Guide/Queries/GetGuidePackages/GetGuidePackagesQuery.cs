using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Features.Guide.Queries.GetGuideById;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Queries.GetGuidePackages;

public class GetGuidePackagesQuery : IRequest<List<GuidePackageSummaryDto>>
{
    public string GuideUserId { get; set; } = string.Empty;
}

public class GetGuidePackagesQueryValidator : AbstractValidator<GetGuidePackagesQuery>
{
    public GetGuidePackagesQueryValidator()
    {
        RuleFor(x => x.GuideUserId).NotEmpty();
    }
}

public class GetGuidePackagesQueryHandler : IRequestHandler<GetGuidePackagesQuery, List<GuidePackageSummaryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetGuidePackagesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<GuidePackageSummaryDto>> Handle(
        GetGuidePackagesQuery request,
        CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .FirstOrDefaultAsync(g => g.UserId == request.GuideUserId, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideUserId);

        var packages = await _context.TourPackages
            .Include(p => p.Availabilities)
            .Include(p => p.Bookings)
            .Where(p => p.GuideProfileId == guide.Id)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);

        return packages.Select(p => new GuidePackageSummaryDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            PricePerPerson = p.PricePerPerson,
            MaxPeople = p.MaxPeople,
            DurationDays = p.DurationDays,
            IncludesFood = p.IncludesFood,
            IncludesTransport = p.IncludesTransport,
            IncludesAccommodation = p.IncludesAccommodation,
            MeetingPoint = p.MeetingPoint,
            MeetingLat = p.MeetingLat,
            MeetingLng = p.MeetingLng,
            TotalBookings = p.Bookings.Count,
            AvailableDates = p.Availabilities
                .Where(a => a.IsAvailable && a.AvailableDate >= DateTime.UtcNow.Date)
                .OrderBy(a => a.AvailableDate)
                .Select(a => a.AvailableDate.ToString("yyyy-MM-dd"))
                .ToList()
        }).ToList();
    }
}