namespace TourGuideBD.Application.Common.Interfaces;

public interface INotificationService
{
    /// <summary>
    /// Sends live visitor count update for a place to all clients subscribed to that place's group.
    /// </summary>
    Task SendLiveVisitorUpdateAsync(int placeId, int currentCount, string crowdLevel, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a weather alert to all users who saved the given district.
    /// </summary>
    Task SendWeatherAlertAsync(int districtId, string title, string message, string severity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a new-place notification to users who saved the district.
    /// </summary>
    Task SendNewPlaceNotificationAsync(int districtId, int placeId, string placeName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a broadcast - districtId null = system-wide.
    /// </summary>
    Task SendBroadcastAsync(int? districtId, string title, string message, CancellationToken cancellationToken = default);

    Task SendGuideLocationUpdateAsync(
    int bookingId,
    double latitude,
    double longitude,
    bool isSharing,
    CancellationToken cancellationToken = default);
}