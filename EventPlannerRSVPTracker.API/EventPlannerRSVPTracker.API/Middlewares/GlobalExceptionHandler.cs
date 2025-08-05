using EventPlannerRSVPTracker.App.DTOs;
using Microsoft.AspNetCore.Diagnostics;

namespace EventPlannerRSVPTracker.API.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception,
            "An unhandled exception occurred: {ErrorMessage}",
            exception.Message);

        var errors = new List<ErrorModel>()
        {
            new ErrorModel("InternalServerError", "Internal Server Error")
        };

        var jsonResponse = JsonResponse.Fail(errors, "An error occurred while processing your request.", 500);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(jsonResponse, cancellationToken);

        return true;
    }
}
