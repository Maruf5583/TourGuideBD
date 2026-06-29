using TourGuideBD.Domain.Entities.Common;
using TourGuideBD.Domain.Entities.PlaceDetails;
using TourGuideBD.Domain.Entities.Reviews;
using TourGuideBD.Domain.Entities.Users;
using TourGuideBD.Domain.Enums;
using TourGuideBD.Domain.ValueObjects;

namespace TourGuideBD.Domain.Entities.Location;

public class Place : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public decimal EntryFee { get; set; }
    public BestSeason BestSeason { get; set; } = BestSeason.AllYear;

    public int DivisionId { get; set; }
    public Division Division { get; set; } = null!;

    public int DistrictId { get; set; }
    public District District { get; set; } = null!;

    public int? UpazilaId { get; set; }
    public Upazila? Upazila { get; set; }

    public string? OpeningHours { get; set; }
    public string? ClosingHours { get; set; }

    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;

    // Whether submitted by a normal user (place discovery feature)
    public string? SubmittedByUserId { get; set; }
    public ApplicationUser? SubmittedByUser { get; set; }

    public double AverageRating { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;

    // Navigation
    public ICollection<PlacePhoto> Photos { get; set; } = new List<PlacePhoto>();
    public ICollection<PlaceCategoryMap> CategoryMaps { get; set; } = new List<PlaceCategoryMap>();
    public ICollection<PlaceTag> Tags { get; set; } = new List<PlaceTag>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public GeoCoordinate Coordinate => new(Latitude, Longitude);
}