using Microsoft.AspNetCore.SignalR;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Infrastructure.Realtime.Hubs;

namespace TourGuideBD.Infrastructure.Realtime;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<LiveVisitorHub> _liveVisitorHub;
    private readonly IHubContext<AlertHub> _alertHub;
    private readonly IHubContext<BroadcastHub> _broadcastHub;

    public SignalRNotificationService(
        IHubContext<LiveVisitorHub> liveVisitorHub,
        IHubContext<AlertHub> alertHub,
        IHubContext<BroadcastHub> broadcastHub)
    {
        _liveVisitorHub = liveVisitorHub;
        _alertHub = alertHub;
        _broadcastHub = broadcastHub;
    }

    public async Task SendLiveVisitorUpdateAsync(int placeId, int currentCount, string crowdLevel, CancellationToken cancellationToken = default)
    {
        await _liveVisitorHub.Clients
            .Group(LiveVisitorHub.GetGroupName(placeId))
            .SendAsync("LiveVisitorUpdated", new { placeId, currentCount, crowdLevel }, cancellationToken);
    }

    public async Task SendWeatherAlertAsync(int districtId, string title, string message, string severity, CancellationToken cancellationToken = default)
    {
        await _alertHub.Clients
            .Group(AlertHub.GetGroupName(districtId))
            .SendAsync("WeatherAlertReceived", new { districtId, title, message, severity }, cancellationToken);
    }

    public async Task SendNewPlaceNotificationAsync(int districtId, int placeId, string placeName, CancellationToken cancellationToken = default)
    {
        await _alertHub.Clients
            .Group(AlertHub.GetGroupName(districtId))
            .SendAsync("NewPlaceAdded", new { districtId, placeId, placeName }, cancellationToken);
    }

    public async Task SendBroadcastAsync(int? districtId, string title, string message, CancellationToken cancellationToken = default)
    {
        if (districtId.HasValue)
        {
            await _broadcastHub.Clients
                .Group(BroadcastHub.GetDistrictGroupName(districtId.Value))
                .SendAsync("BroadcastReceived", new { districtId, title, message }, cancellationToken);
        }
        else
        {
            await _broadcastHub.Clients
                .Group(BroadcastHub.SystemWideGroup)
                .SendAsync("BroadcastReceived", new { districtId = (int?)null, title, message }, cancellationToken);
        }


    }

    public async Task SendGuideLocationUpdateAsync(
    int bookingId,
    double latitude,
    double longitude,
    bool isSharing,
    CancellationToken cancellationToken = default)
    {
        await _liveVisitorHub.Clients
            .Group($"booking-{bookingId}")
            .SendAsync("GuideLocationUpdated", new
            {
                bookingId,
                latitude,
                longitude,
                isSharing,
                updatedAt = DateTime.UtcNow
            }, cancellationToken);
    }
}