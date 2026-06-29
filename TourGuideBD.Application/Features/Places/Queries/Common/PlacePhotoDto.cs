namespace TourGuideBD.Application.Features.Places.Queries.Common;

public class PlacePhotoDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public bool IsCover { get; set; }
}