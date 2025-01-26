using Microsoft.AspNetCore.Mvc;
using Vision360.Context.Models;

namespace Vision360.Context.Repositories.Interfaces;

public interface IAdminRepository
{
    Task<IEnumerable<User>> GetUsersAsync(string search, string sortColumn, string sortDirection);
    
    Task BlockUserAsync(IEnumerable<string> userIds);
    
    Task UnBlockUserAsync(IEnumerable<string> userIds);
    
    Task DeleteUserAsync(IEnumerable<string> userIds);
}