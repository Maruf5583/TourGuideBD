namespace TourGuideBD.Application.Features.Places.Queries.Common;

public class PlaceListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameBn { get; set; } = string.Empty;
    public string? CoverPhotoUrl { get; set; }
    public string DistrictName { get; set; } = string.Empty;
    public string DivisionName { get; set; } = string.Empty;
    public decimal EntryFee { get; set; }
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public List<string> Categories { get; set; } = new();
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    // populated only for nearby search results
    public double? DistanceKm { get; set; }
}