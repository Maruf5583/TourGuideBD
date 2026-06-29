using Microsoft.AspNetCore.Identity;
using TourGuideBD.Application.Common.Interfaces;
using TourGuideBD.Application.Common.Models;
using TourGuideBD.Domain.Entities.Users;

namespace TourGuideBD.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string email, string password, string fullName)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            EmailConfirmed = true, // simplify - no email confirmation flow for now
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        return (result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description)), user.Id);
    }

    public async Task<bool> CheckPasswordAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<string?> GetUserIdByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user?.Id;
    }

    public async Task<IList<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new List<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<Result> AddUserToRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return Result.Failure("User not found.");

        if (await _userManager.IsInRoleAsync(user, role))
            return Result.Success();

        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }

    public async Task<Result> RemoveUserFromRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return Result.Failure("User not found.");

        var result = await _userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }

    public async Task<Result> SetBanStatusAsync(string userId, bool isBanned)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return Result.Failure("User not found.");

        user.IsBanned = isBanned;

        // Lock account when banned (LockoutEnd far future), unlock when unbanned
        if (isBanned)
        {
            user.LockoutEnabled = true;
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }
        else
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
        }

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(e => e.Description));
    }

    public async Task<bool> IsUserBannedAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.IsBanned ?? false;
    }
}