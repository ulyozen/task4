using Microsoft.AspNetCore.Identity;
using Vision360.Context.Models;
using Vision360.Context.Repositories.Interfaces;
using Vision360.Mappers;
using Vision360.Models;

namespace Vision360.Context.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthRepository(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<IdentityResult> RegisterAsync(AuthViewModel model)
    {
        return await _userManager.CreateAsync(model.MapToUser(), model.Password!);
    }
    
    public async Task<IdentityResult> LoginAsync(AuthViewModel model)
    {
        var isValidUser = await ValidateUserAsync(model);
        if (!isValidUser.Succeeded)
            return isValidUser;
        
        var signInResult = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, lockoutOnFailure: false);
        if (signInResult.IsLockedOut)
            return IdentityResult.Failed(new IdentityError { Code = "IsLockedOut", Description = "The email address is locked out." });
        
        await UpdateLastSeen(model);
        
        return IdentityResult.Success;
    }
    
    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    private async Task<IdentityResult> ValidateUserAsync(AuthViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email!);
        if (user == null)
            return IdentityResult.Failed(new IdentityError { Code = "UserNotFound", Description = "The email address does not exist." });
        
        var isPasswordValid  = await _userManager.CheckPasswordAsync(user, model.Password!);
        if (!isPasswordValid)
            return IdentityResult.Failed(new IdentityError { Code = "InvalidPassword", Description = "The password is incorrect" });

        return IdentityResult.Success;
    }
    
    private async Task UpdateLastSeen(AuthViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email!);
        if (user != null)
        {
            user.LastLoginTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }
    }
}