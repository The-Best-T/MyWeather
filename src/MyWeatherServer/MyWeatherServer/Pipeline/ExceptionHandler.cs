using System.ComponentModel.DataAnnotations;
using Core;
using Exceptions;

namespace MyWeatherServer.Pipeline;

internal class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = GetStatusCode(exception);

        await context.Response.WriteAsync(new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message,
        }.ToString() ?? string.Empty);
    }

    private static int GetStatusCode(
        Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => StatusCodes.Status400BadRequest,
            EntityNotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            OperationCanceledException => 499,
            _ => StatusCodes.Status500InternalServerError,
        };
    }
}