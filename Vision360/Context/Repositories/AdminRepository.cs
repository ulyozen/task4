using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vision360.Context.Models;
using Vision360.Context.Repositories.Interfaces;

namespace Vision360.Context.Repositories;

public class AdminRepository(UserManager<User> userManager, SignInManager<User> signInManager) : IAdminRepository
{
    public async Task<IEnumerable<User>> GetUsersAsync(string search, string sortColumn, string sortDirection)
    {
        var users = await userManager.Users.ToListAsync();

        return SortUsers(FilterUsers(users, search), sortColumn, sortDirection);
    }

    public async Task BlockUserAsync(IEnumerable<string> userIds)
    {
        foreach (var userId  in userIds)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnd = DateTimeOffset.MaxValue;
                
                var result = await userManager.UpdateAsync(user);
            }
        }
    }
    
    public async Task UnBlockUserAsync(IEnumerable<string> userIds)
    {
        foreach (var userId  in userIds)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnd = null;
                
                await userManager.UpdateAsync(user);
            }
        }
    }

    public async Task DeleteUserAsync(IEnumerable<string> userIds)
    {
        foreach (var userId  in userIds)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
        }
    }
    
    private static IEnumerable<User> FilterUsers(IEnumerable<User> users, string search)
    {
        if (string.IsNullOrEmpty(search)) return users;

        return users.Where(u =>
            u.Name!.Contains(search, StringComparison.OrdinalIgnoreCase) ||
            u.Email!.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
    
    private static IEnumerable<User> SortUsers(IEnumerable<User> users, string sortColumn, string sortDirection)
    {
        return sortColumn switch
        {
            "Name" => sortDirection == "asc" 
                ? users.OrderBy(u => u.Name) 
                : users.OrderByDescending(u => u.Name),
            "Email" => sortDirection == "asc" 
                ? users.OrderBy(u => u.Email) 
                : users.OrderByDescending(u => u.Email),
            "LastSeen" => sortDirection == "asc" 
                ? users.OrderBy(u => u.LastLoginTime) 
                : users.OrderByDescending(u => u.LastLoginTime),
            _ => users
        };
    }
}