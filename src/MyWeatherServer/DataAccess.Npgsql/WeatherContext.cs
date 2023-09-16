#pragma warning disable

using DataAccess.Npgsql.DatabaseEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Npgsql;

public class WeatherContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Location> Locations { get; set; }

    public WeatherContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(WeatherContext).Assembly);
    }
}