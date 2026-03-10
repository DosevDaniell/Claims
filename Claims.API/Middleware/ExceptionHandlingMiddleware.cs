using Claims.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Claims.API.Middleware;

/// <summary>
/// Handles application exceptions
/// </summary>
public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await WriteProblemDetails(
               context,
               StatusCodes.Status400BadRequest,
               "Validation failed",
               "One or more validation errors occurred.",
               ex.Errors);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemDetails(
               context,
               StatusCodes.Status404NotFound,
               "Not found",
               ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");

            await WriteProblemDetails(
                 context,
                 StatusCodes.Status404NotFound,
                 "Not found",
                 ex.Message);
        }
    }
    private static async Task WriteProblemDetails(
        HttpContext context,
        int statusCode,
        string title,
        string detail,
        IReadOnlyList<string>? errors = null)
    {
        context.Response.StatusCode = statusCode;

        var problem = new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };

        if (errors is not null)
            problem.Extensions["errors"] = errors;

        await context.Response.WriteAsJsonAsync(problem);
    }
}