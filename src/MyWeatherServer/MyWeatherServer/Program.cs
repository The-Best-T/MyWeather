using MyWeatherServer.Pipeline;
using System.Reflection;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

var app = builder.AddApiServices().Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
