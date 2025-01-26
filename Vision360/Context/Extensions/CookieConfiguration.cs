namespace Vision360.Context.Extensions;

public static class CookieConfiguration
{
    public static void AddCookieConfiguration(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Login";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.SlidingExpiration = false;
            options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        });
    }
}