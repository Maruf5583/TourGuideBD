using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Application.Features.Guide.Queries.GetGuideById;

public class GuideReviewDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int OverallRating { get; set; }
    public int PunctualityRating { get; set; }
    public int KnowledgeRating { get; set; }
    public int CommunicationRating { get; set; }
    public int SafetyRating { get; set; }
    public int ValueRating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GuideAvailabilityDto
{
    public string Date { get; set; } = string.Empty;
    public int MaxBookings { get; set; }
    public int CurrentBookings { get; set; }
    public int RemainingSlots { get; set; }
    public bool IsAvailable { get; set; }
}

public class GuidePackageSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal PricePerPerson { get; set; }
    public int MaxPeople { get; set; }
    public int DurationDays { get; set; }
    public bool IncludesFood { get; set; }
    public bool IncludesTransport { get; set; }
    public bool IncludesAccommodation { get; set; }
    public string? AdditionalIncludes { get; set; }
    public string MeetingPoint { get; set; } = string.Empty;
    public double MeetingLat { get; set; }
    public double MeetingLng { get; set; }
    public string MeetingGoogleMapsUrl { get; set; } = string.Empty;
    public int TotalBookings { get; set; }
    public int CompletedBookings { get; set; }

    public List<string> AvailableDates { get; set; } = new();
    public List<GuideAvailabilityDto> UpcomingAvailabilities { get; set; } = new();

    // Places included
    public List<int> PlaceIds { get; set; } = new();
}

public class GuideDetailDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string ProfilePhotoUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Languages { get; set; } = string.Empty;
    public string Specialities { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public string Badge { get; set; } = string.Empty;

    // Stats
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public int TotalToursCompleted { get; set; }
    public int ActivePackages { get; set; }

    // Districts the guide operates in
    public List<int> OperatingDistrictIds { get; set; } = new();

    // Packages
    public List<GuidePackageSummaryDto> Packages { get; set; } = new();

    // Reviews
    public List<GuideReviewDto> RecentReviews { get; set; } = new();
}

public class GetGuideByIdQuery : IRequest<GuideDetailDto>
{
    public int GuideId { get; set; }
}

public class GetGuideByIdQueryValidator : AbstractValidator<GetGuideByIdQuery>
{
    public GetGuideByIdQueryValidator()
    {
        RuleFor(x => x.GuideId).GreaterThan(0);
    }
}

public class GetGuideByIdQueryHandler : IRequestHandler<GetGuideByIdQuery, GuideDetailDto>
{
    private readonly IApplicationDbContext _context;

    public GetGuideByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GuideDetailDto> Handle(
        GetGuideByIdQuery request,
        CancellationToken cancellationToken)
    {
        var guide = await _context.GuideProfiles
            .Include(g => g.TourPackages.Where(p => p.IsActive))
                .ThenInclude(p => p.Availabilities)
            .Include(g => g.TourPackages.Where(p => p.IsActive))
                .ThenInclude(p => p.Bookings)
            .Include(g => g.Reviews.OrderByDescending(r => r.CreatedAt).Take(10))
            .FirstOrDefaultAsync(g => g.Id == request.GuideId && g.IsActive, cancellationToken);

        if (guide == null)
            throw new NotFoundException("GuideProfile", request.GuideId);

        // District IDs deserialize করো
        var districtIds = new List<int>();
        try
        {
            districtIds = JsonSerializer.Deserialize<List<int>>(guide.OperatingDistrictIds)
                ?? new List<int>();
        }
        catch { }

        // Reviews user name পেতে user load করো
        var userIds = guide.Reviews.Select(r => r.UserId).Distinct().ToList();
        var users = await _context.Users
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new { u.Id, u.FullName })
            .ToListAsync(cancellationToken);

        return new GuideDetailDto
        {
            Id = guide.Id,
            UserId = guide.UserId,
            FullName = guide.FullName,
            ProfilePhotoUrl = guide.ProfilePhotoUrl,
            Bio = guide.Bio,
            Languages = guide.Languages,
            Specialities = guide.Specialities,
            ExperienceYears = guide.ExperienceYears,
            Badge = guide.Badge.ToString(),
            AverageRating = Math.Round(guide.AverageRating, 1),
            TotalReviews = guide.TotalReviews,
            TotalToursCompleted = guide.TotalToursCompleted,
            ActivePackages = guide.TourPackages.Count,
            OperatingDistrictIds = districtIds,

            Packages = guide.TourPackages.Select(p =>
            {
                var placeIds = new List<int>();
                try { placeIds = JsonSerializer.Deserialize<List<int>>(p.PlaceIds) ?? new(); }
                catch { }

                return new GuidePackageSummaryDto
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
                    AdditionalIncludes = p.AdditionalIncludes,
                    MeetingPoint = p.MeetingPoint,
                    MeetingLat = p.MeetingLat,
                    MeetingLng = p.MeetingLng,
                    MeetingGoogleMapsUrl =
                        $"https://www.google.com/maps/search/?api=1&query={p.MeetingLat},{p.MeetingLng}",
                    TotalBookings = p.Bookings.Count,
                    CompletedBookings = p.Bookings
                        .Count(b => b.Status == BookingStatus.Completed),
                    PlaceIds = placeIds,
                    UpcomingAvailabilities = p.Availabilities
                        .Where(a => a.AvailableDate >= DateTime.UtcNow.Date)
                        .OrderBy(a => a.AvailableDate)
                        .Take(10)
                        .Select(a => new GuideAvailabilityDto
                        {
                            Date = a.AvailableDate.ToString("yyyy-MM-dd"),
                            MaxBookings = a.MaxBookings,
                            CurrentBookings = a.CurrentBookings,
                            RemainingSlots = a.MaxBookings - a.CurrentBookings,
                            IsAvailable = a.IsAvailable
                        }).ToList()
                };
            }).ToList(),

            RecentReviews = guide.Reviews.Select(r => new GuideReviewDto
            {
                Id = r.Id,
                UserName = users.FirstOrDefault(u => u.Id == r.UserId)?.FullName ?? "Anonymous",
                OverallRating = r.OverallRating,
                PunctualityRating = r.PunctualityRating,
                KnowledgeRating = r.KnowledgeRating,
                CommunicationRating = r.CommunicationRating,
                SafetyRating = r.SafetyRating,
                ValueRating = r.ValueRating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }
}