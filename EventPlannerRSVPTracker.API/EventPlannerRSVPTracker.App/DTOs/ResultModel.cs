namespace EventPlannerRSVPTracker.App.DTOs;

public class ResultModel
{
    public bool IsSuccess { get; protected set; } = false;

    public int Status { get; set; } = 500;

    public List<ErrorModel>? ErrorDetails { get; protected set; }

    public static ResultModel Success(int status = 200) => new() { IsSuccess = true, Status = status };

    public static ResultModel Fail(List<ErrorModel> errorDetails, int status = 500) => new() { ErrorDetails = errorDetails, Status = status };
}

public class ResultModel<T> : ResultModel
{
    public T? Data { get; private set; }

    public static ResultModel<T> Success(T data, int status = 200) => new() { Data = data, IsSuccess = true, Status = status };

    public new static ResultModel<T> Fail(List<ErrorModel> errorDetails, int status = 500) => new() { ErrorDetails = errorDetails, Status = status };
}

public class ErrorModel
{
    public ErrorModel(string type, string message)
    {
        Type = type;
        Message = message;
    }

    /// <summary>
    /// Type of exception/error occurred
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// A summary of the error
    /// </summary>
    public string Message { get; set; }
}
