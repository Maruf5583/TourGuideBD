
using Microsoft.AspNetCore.Identity;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.Reviews;
using TourGuideBD.Domain.Entities.Trip;

namespace TourGuideBD.Domain.Entities.Users;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }

    public bool IsBanned { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Refresh token rotation - latest token id stored for reference (actual token in Redis)
    public string? CurrentRefreshTokenId { get; set; }

    public ICollection<FavouritePlace> FavouritePlaces { get; set; } = new List<FavouritePlace>();
    public ICollection<VisitHistory> VisitHistories { get; set; } = new List<VisitHistory>();
    public ICollection<CheckIn> CheckIns { get; set; } = new List<CheckIn>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Itinerary> Itineraries { get; set; } = new List<Itinerary>();
    public ICollection<Place> SubmittedPlaces { get; set; } = new List<Place>();
    public ICollection<SavedDistrict> SavedDistricts { get; set; } = new List<SavedDistrict>();
}