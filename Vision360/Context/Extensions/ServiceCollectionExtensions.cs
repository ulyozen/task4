using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Vision360.Context.Models;
using Vision360.Exceptions;

namespace Vision360.Context;

public static class ServiceCollectionExtensions
{
    public static void AddDbSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConnectionOptions>(config =>
        {
            config.User = GetEnvironmentVariable(configuration, "POSTGRES_USER");
            
            config.Password = GetEnvironmentVariable(configuration, "POSTGRES_PASSWORD");
            
            config.Host = GetEnvironmentVariable(configuration, "POSTGRES_HOST");
            
            if (!int.TryParse(configuration["POSTGRES_PORT"], out var port))
            {
                throw new ConfigurationException($"Invalid {nameof(config.Port)} environment variable. Must be a valid integer.");
            }
            config.Port = port;

            config.Database = GetEnvironmentVariable(configuration, "POSTGRES_DB");
        });
        
        services.AddDbContext<VisionDbContext>((provider, options) =>
        {
            var env = provider.GetRequiredService<IOptions<DatabaseConnectionOptions>>().Value;

            options.UseNpgsql(BuildConnectionString(env));
        });
    }
    
    public static void AddCustomIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<VisionDbContext>()
            .AddDefaultTokenProviders();
    }
    
    private static string GetEnvironmentVariable(IConfiguration configuration, string key)
    {
        return configuration[key] ?? throw new ConfigurationException($"The environment variable {key} is missing.");
    }
    
    private static string BuildConnectionString(DatabaseConnectionOptions env)
    {
        return $"User ID={env.User};Password={env.Password};Host={env.Host};Port={env.Port};Database={env.Database};";
    }
}