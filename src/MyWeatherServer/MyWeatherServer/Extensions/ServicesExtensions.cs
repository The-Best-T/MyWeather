using DataAccess.Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyWeatherServer.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection ConfigureControllers(
        this IServiceCollection services)
    {
        services
            .AddControllers(config =>
            {
                config.ReturnHttpNotAcceptable = true;
                config.RespectBrowserAcceptHeader = true;
            })
            .AddXmlDataContractSerializerFormatters();

        return services;
    }

    public static IServiceCollection ConfigureIdentity(
        this IServiceCollection services)
    {
        services
            .AddIdentityCore<IdentityUser>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireDigit = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<WeatherContext>();

        return services;
    }

    public static IServiceCollection ConfigureNpgsqlContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<WeatherContext>(opts =>
        {
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b =>
                {
                    b.MigrationsAssembly(typeof(WeatherContext).Assembly.FullName);
                });
        });

        return services;
    }

    public static IServiceCollection ConfigureDbServices(
        this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection ConfigureValidation(
        this IServiceCollection services)
    {
        return services;
    }
}