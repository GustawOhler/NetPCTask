using System.Collections.Generic;

public class OperationResult<T>
{
    private OperationResult(T? value, bool success, bool notFound, bool unathorized, IDictionary<string, string[]>? validationErrors)
    {
        Value = value;
        Success = success;
        NotFound = notFound;
        ValidationErrors = validationErrors;
    }

    public T? Value { get; }

    public bool Success { get; }

    public bool NotFound { get; }
    public bool Unauthorized { get; }

    public IDictionary<string, string[]>? ValidationErrors { get; }

    public static OperationResult<T> Successful(T value) => new(value, true, false, false, null);

    public static OperationResult<T> Successful() => new(default, true, false, false, null);

    public static OperationResult<T> NotFoundResult() => new(default, false, true, false, null);
    public static OperationResult<T> UnauthorizedResult() => new(default, false, false, true, null);

    public static OperationResult<T> ValidationFailed(IDictionary<string, string[]> errors) => new(default, false, false, false, errors);
}
