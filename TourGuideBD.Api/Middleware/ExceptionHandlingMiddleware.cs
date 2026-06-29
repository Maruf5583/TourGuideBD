
using System.Net;
using System.Text.Json;
using TourGuideBD.Domain.Exceptions;

namespace TourGuideBD.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errors) = exception switch
        {
            ValidationException validationEx => (HttpStatusCode.BadRequest, (object)validationEx.Errors),
            NotFoundException notFoundEx => (HttpStatusCode.NotFound, (object)new { message = notFoundEx.Message }),
            ForbiddenAccessException forbiddenEx => (HttpStatusCode.Forbidden, (object)new { message = forbiddenEx.Message }),
            _ => (HttpStatusCode.InternalServerError, (object)new { message = "An unexpected error occurred." })
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred.");
        }

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            errors
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}