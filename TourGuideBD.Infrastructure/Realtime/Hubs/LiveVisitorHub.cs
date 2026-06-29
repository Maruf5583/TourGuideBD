using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace TourGuideBD.Infrastructure.Realtime.Hubs;

public class LiveVisitorHub : Hub
{
    public async Task JoinPlaceGroup(int placeId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(placeId));
    }

    public async Task LeavePlaceGroup(int placeId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(placeId));
    }

    public static string GetGroupName(int placeId) => $"place-{placeId}";
}