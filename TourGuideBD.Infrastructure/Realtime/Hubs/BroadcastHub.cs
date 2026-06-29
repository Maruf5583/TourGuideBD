using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace TourGuideBD.Infrastructure.Realtime.Hubs;

public class BroadcastHub : Hub
{
    public const string SystemWideGroup = "system-wide";

    public async Task JoinSystemWide()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, SystemWideGroup);
    }

    public async Task JoinDistrictGroup(int districtId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetDistrictGroupName(districtId));
    }

    public static string GetDistrictGroupName(int districtId) => $"broadcast-district-{districtId}";
}