using System.Text;
using Core;
using DataAccess.Npgsql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MyWeatherServer.Pipeline;

internal static class ServicesExtensions
{
    public static WebApplicationBuilder AddApiServices(
        this WebApplicationBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", false, true)
                            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                                false, true)
                            .Build();

        builder.Services
            .ConfigureAuthentication(configuration)
            .AddAuthorization()
            .ConfigureControllers()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .ConfigureNpgsqlContext(configuration)
            .ConfigureIdentity()
            .ConfigureDbServices()
            .AddMediatR(typeof(Program))
            .ConfigureValidation()
            .AddAutoMapper(typeof(Program));

        return builder;
    }

    private static IServiceCollection ConfigureControllers(
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

    private static IServiceCollection ConfigureIdentity(
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

    private static IServiceCollection ConfigureAuthentication(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        if (jwtSettings is null)
            throw new ArgumentNullException(nameof(configuration), $"{nameof(jwtSettings)} is null");
        services.AddSingleton(jwtSettings);
        
        var key = Environment.GetEnvironmentVariable(GlobalConstants.SecretKeyName);

        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });

        return services;
    }

    private static IServiceCollection ConfigureNpgsqlContext(
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

    private static IServiceCollection ConfigureDbServices(
        this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection ConfigureValidation(
        this IServiceCollection services)
    {
        return services;
    }
}