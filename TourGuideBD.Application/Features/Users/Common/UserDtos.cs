using TourGuideBD.Application.Features.Places.Queries.Common;

namespace TourGuideBD.Application.Features.Users.Common;

public class UserProfileDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<string> Roles { get; set; } = new();
}

public class FavouritePlaceDto
{
    public int FavouriteId { get; set; }
    public DateTime AddedAt { get; set; }
    public PlaceListItemDto Place { get; set; } = null!;
}

public class FavouritesByDistrictDto
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; } = string.Empty;
    public List<FavouritePlaceDto> Places { get; set; } = new();
}

public class VisitHistoryDto
{
    public int Id { get; set; }
    public int PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public string? PlaceCoverPhotoUrl { get; set; }
    public DateTime VisitedAt { get; set; }
}

public class CheckInDto
{
    public int Id { get; set; }
    public int PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public string? PlaceCoverPhotoUrl { get; set; }
    public string? Note { get; set; }
    public DateTime CheckedInAt { get; set; }
}

public class SavedDistrictDto
{
    public int DistrictId { get; set; }
    public string DistrictName { get; set; } = string.Empty;
    public string DistrictNameBn { get; set; } = string.Empty;
}