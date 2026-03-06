using Claims.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Claims.API.Middleware;

public sealed class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Validation failed",
                Status = 400,
                Detail = "One or more validation errors occurred.",
                Extensions = { ["errors"] = ex.Errors }
            });
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Not found",
                Status = 404,
                Detail = ex.Message
            });
        }
    }
}