using MyWeatherServer.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                        false, true)
                    .Build();


services
    .ConfigureControllers()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .ConfigureNpgsqlContext(configuration)
    .ConfigureIdentity()
    .ConfigureDbServices();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
