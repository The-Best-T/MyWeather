using MyWeatherServer.Pipeline;

var builder = WebApplication.CreateBuilder(args);

var app = builder.AddApiServices().Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
