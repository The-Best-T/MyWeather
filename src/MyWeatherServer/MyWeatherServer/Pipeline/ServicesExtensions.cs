using System.Text;
using DataAccess.Npgsql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MyWeatherServer.Pipeline;

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

    public static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var key = Environment.GetEnvironmentVariable("IDKEY");

        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidAudience = configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });

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