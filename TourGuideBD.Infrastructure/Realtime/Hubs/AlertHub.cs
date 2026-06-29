using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace TourGuideBD.Infrastructure.Realtime.Hubs;

public class AlertHub : Hub
{
    public async Task JoinDistrictGroup(int districtId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(districtId));
    }

    public async Task LeaveDistrictGroup(int districtId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(districtId));
    }

    public static string GetGroupName(int districtId) => $"district-{districtId}";
}