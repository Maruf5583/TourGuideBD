using TourGuideBD.Application.Common.Models;

namespace TourGuideBD.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(Result Result, string UserId)> CreateUserAsync(string email, string password, string fullName);
    Task<bool> CheckPasswordAsync(string email, string password);
    Task<string?> GetUserIdByEmailAsync(string email);
    Task<IList<string>> GetUserRolesAsync(string userId);
    Task<Result> AddUserToRoleAsync(string userId, string role);
    Task<Result> RemoveUserFromRoleAsync(string userId, string role);
    Task<Result> SetBanStatusAsync(string userId, bool isBanned);
    Task<bool> IsUserBannedAsync(string userId);
}