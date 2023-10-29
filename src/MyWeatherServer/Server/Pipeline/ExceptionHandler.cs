using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Text.Json;
using Core;
using Exceptions;

namespace Server.Pipeline;

internal class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var exceptionResponse = new ErrorDetails
            {
                Message = ex.Message,
            };

            var statusCode = ex switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                EntityNotFoundException => StatusCodes.Status404NotFound,
                EntityConflictException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError,
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            var json = JsonSerializer.Serialize(exceptionResponse);

            await context.Response.WriteAsync(json, context.RequestAborted);
        }
    }
}