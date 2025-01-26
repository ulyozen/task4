using Microsoft.AspNetCore.Identity;
using Vision360.Models;

namespace Vision360.Context.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<IdentityResult> RegisterAsync(AuthViewModel model);
    
    Task<IdentityResult> LoginAsync(AuthViewModel model);
    
    Task LogoutAsync();
}