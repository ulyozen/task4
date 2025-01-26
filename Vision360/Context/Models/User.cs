using Microsoft.AspNetCore.Identity;

namespace Vision360.Context.Models;

public class User : IdentityUser
{
    public string? Name { get; set; }
    
    public DateTime? LastLoginTime { get; set; }
    
    public bool IsActive => LockoutEnd == null || LockoutEnd <= DateTime.UtcNow;
}