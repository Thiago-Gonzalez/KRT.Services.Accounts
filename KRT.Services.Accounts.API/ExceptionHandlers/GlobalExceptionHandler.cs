using KRT.Services.Accounts.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace KRT.Services.Accounts.API.ExceptionHandlers;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred.");

        int statusCode;
        string message;

        switch (exception)
        {
            case BusinessRuleValidationException businessException:
                statusCode = StatusCodes.Status400BadRequest;
                message = businessException.Message;
                break;

            case ArgumentException argumentException:
                statusCode = StatusCodes.Status400BadRequest;
                message = argumentException.Message;
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = _environment.IsDevelopment()
                    ? exception.Message
                    : "Ocorreu um erro interno no servidor.";
                break;
        }

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsJsonAsync(
            new { message },
            cancellationToken);

        return true;
    }
}
