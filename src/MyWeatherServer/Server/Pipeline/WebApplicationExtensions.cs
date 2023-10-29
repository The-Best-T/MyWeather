using DataAccess.Npgsql;
using Microsoft.EntityFrameworkCore;

namespace Server.Pipeline;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> MigrateDatabase(
        this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;

        try
        {
            var db = scopedServices.GetRequiredService<WeatherContext>();

            await db.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error with migrations: {0}", ex);
        }

        return app;
    }
}