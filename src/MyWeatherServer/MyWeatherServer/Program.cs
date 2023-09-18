using MyWeatherServer.Pipeline;
using System.Reflection;
using MediatR;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                        false, true)
                    .Build();


services
    .ConfigureAuthentication(configuration)
    .AddAuthorization()
    .ConfigureControllers()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .ConfigureNpgsqlContext(configuration)
    .ConfigureIdentity()
    .ConfigureDbServices()
    .AddMediatR(typeof(Program))
    .AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
