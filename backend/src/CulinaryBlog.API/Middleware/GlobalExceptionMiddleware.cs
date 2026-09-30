using CulinaryBlog.Application.Common.Exceptions;
using CulinaryBlog.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlog.API.Middleware;

/// <summary>
/// Bắt mọi exception chưa được xử lý trong pipeline và trả về RFC 7807
/// (application/problem+json). Endpoint chỉ cần throw, không tự try/catch.
/// </summary>
public sealed class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context, IProblemDetailsService problemDetailsService)
    {
        try
        {
            await _next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            // Client đã ngắt kết nối, không còn ai để nhận response.
            _logger.LogDebug("Request {Method} {Path} was cancelled by the client.",
                context.Request.Method, context.Request.Path);
        }
        catch (Exception exception)
        {
            if (context.Response.HasStarted)
            {
                _logger.LogError(exception, "Unhandled exception after the response has started.");
                throw;
            }

            var problem = CreateProblemDetails(exception);
            Log(exception, problem.Status!.Value);

            context.Response.Clear();
            context.Response.StatusCode = problem.Status.Value;

            await problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problem,
                Exception = exception
            });
        }
    }

    private ProblemDetails CreateProblemDetails(Exception exception) => exception switch
    {
        ValidationException ex => new HttpValidationProblemDetails(
            ex.Errors.ToDictionary(e => e.Key, e => e.Value))
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "One or more validation errors occurred.",
            Detail = ex.Message
        },
        NotFoundException ex => Problem(StatusCodes.Status404NotFound, "Resource not found.", ex.Message),
        ForbiddenException ex => Problem(StatusCodes.Status403Forbidden, "Forbidden.", ex.Message),
        ConflictException ex => Problem(StatusCodes.Status409Conflict, "Conflict.", ex.Message),
        DbUpdateConcurrencyException => Problem(
            StatusCodes.Status409Conflict,
            "Conflict.",
            "The resource was modified by another request. Reload it and try again."),
        DomainException ex => Problem(StatusCodes.Status422UnprocessableEntity, "Business rule violation.", ex.Message),
        BadHttpRequestException ex => Problem(ex.StatusCode, "Bad request.", ex.Message),
        // Không lộ chi tiết lỗi hệ thống ra ngoài môi trường Development.
        _ => Problem(
            StatusCodes.Status500InternalServerError,
            "An unexpected error occurred.",
            _environment.IsDevelopment() ? exception.Message : null)
    };

    private static ProblemDetails Problem(int status, string title, string? detail) => new()
    {
        Status = status,
        Title = title,
        Detail = detail
    };

    private void Log(Exception exception, int statusCode)
    {
        if (statusCode >= StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        else
            _logger.LogInformation("Request failed with {StatusCode}: {Message}", statusCode, exception.Message);
    }
}
