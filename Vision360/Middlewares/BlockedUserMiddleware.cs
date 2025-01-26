using Microsoft.AspNetCore.Identity;
using Vision360.Context.Models;

namespace Vision360.Middlewares;

public class BlockedUserMiddleware : IMiddleware
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public BlockedUserMiddleware(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var isAuthenticated = context.User.Identity!.IsAuthenticated;
        if (isAuthenticated)
        {
            var user = await _userManager.GetUserAsync(context.User);
            if (user == null || (user.LockoutEnd > DateTimeOffset.UtcNow))
            {
                await _signInManager.SignOutAsync();

                context.Response.Redirect("/Auth/Login");
                return;
            }
        }

        await next(context);
    }
}