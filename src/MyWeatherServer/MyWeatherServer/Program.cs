using MyWeatherServer.Pipeline;

var builder = WebApplication.CreateBuilder(args);

var app = await builder
                .AddApiServices()
                .Build()
                .MigrateDatabase();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandler>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();