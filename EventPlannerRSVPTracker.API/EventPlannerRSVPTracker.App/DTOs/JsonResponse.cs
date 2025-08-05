using System.Net;

namespace EventPlannerRSVPTracker.App.DTOs;

public class JsonResponse
{
    public bool IsSuccess { get; private set; }

    public string? Message { get; private set; }

    public object? Data { get; private set; }

    public object? ErrorDetails { get; private set; }

    public int Status { get; private set; }

    public static JsonResponse Success(string? message = null,
        object? data = null,
        int status = ((int)HttpStatusCode.OK))
    {
        return new JsonResponse()
        {
            IsSuccess = true,
            Message = message,
            Data = data,
            ErrorDetails = null,
            Status = status,
        };
    }

    public static JsonResponse Fail(object? errorDetails,
        string? message = null,
        int status = ((int)HttpStatusCode.BadRequest))
    {
        return new JsonResponse()
        {
            IsSuccess = false,
            Message = message,
            Data = null,
            ErrorDetails = errorDetails,
            Status = status,
        };
    }
}
