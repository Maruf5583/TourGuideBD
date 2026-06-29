using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Domain.Enums;

namespace TourGuideBD.Application.Features.Places.Queries.GetPlaceDetail;

public class PlaceDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public decimal EntryFee { get; set; }
    public BestSeason BestSeason { get; set; }

    public string DivisionName { get; set; } = string.Empty;
    public string DistrictName { get; set; } = string.Empty;
    public string? UpazilaName { get; set; }

    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }

    public string? OpeningHours { get; set; }
    public string? ClosingHours { get; set; }

    public List<PlacePhotoDto> Photos { get; set; } = new();
    public List<string> Categories { get; set; } = new();
    public List<string> Tags { get; set; } = new();
}