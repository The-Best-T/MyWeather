using System.Reflection;
using System.Text;
using Core;
using DataAccess.Npgsql;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UseCases.Abstractions;
using UseCases.Services;

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

        var assemblies = new[]
        {
            Assembly.GetExecutingAssembly(),
            Assembly.Load("Controllers"), 
            Assembly.Load("UseCases"),
            Assembly.Load("DataAccess.Npgsql"),
        };

        builder.Services
               .ConfigureAuthentication(configuration)
               .AddAuthorization()
               .ConfigureControllers()
               .AddEndpointsApiExplorer()
               .AddSwaggerGen()
               .ConfigureNpgsqlContext(configuration)
               .ConfigureIdentity()
               .ConfigureDbServices()
               .ConfigureLogicServices()
               .AddMediatR(assemblies)
               .ConfigureValidation()
               .AddAutoMapper(assemblies)
               .ConfigureSwagger();

        return builder;
    }

    private static IServiceCollection ConfigureControllers(
        this IServiceCollection services)
    {
        services
            .AddControllers(config =>
            {
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

    private static IServiceCollection ConfigureLogicServices(
        this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }

    private static IServiceCollection ConfigureSwagger(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(cfg =>
        {
            cfg.CustomSchemaIds(x => x.FullName);

            cfg.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MyWeather API",
                Description = "An API to look ur weather",
                Contact = new OpenApiContact
                {
                    Name = "Evgeniy Hamichenok",
                    Email = "zhamichenok@gmail.com",
                    Url = new Uri("https://github.com/The-Best-T"),
                }
            });

            cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
            });

            cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                        Name = "Bearer",
                    },
                    new List<string>()
                }
            });
        });

        return services;
    }

    private static IServiceCollection ConfigureValidation(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.Load("UseCases"));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));

        return services;
    }
}