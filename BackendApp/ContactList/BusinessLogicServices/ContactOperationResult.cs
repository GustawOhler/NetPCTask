using System.Collections.Generic;

public class ContactOperationResult<T>
{
    private ContactOperationResult(T? value, bool success, bool notFound, IDictionary<string, string[]>? validationErrors)
    {
        Value = value;
        Success = success;
        NotFound = notFound;
        ValidationErrors = validationErrors;
    }

    public T? Value { get; }

    public bool Success { get; }

    public bool NotFound { get; }

    public IDictionary<string, string[]>? ValidationErrors { get; }

    public static ContactOperationResult<T> Successful(T value) => new(value, true, false, null);

    public static ContactOperationResult<T> Successful() => new(default, true, false, null);

    public static ContactOperationResult<T> NotFoundResult() => new(default, false, true, null);

    public static ContactOperationResult<T> ValidationFailed(IDictionary<string, string[]> errors) => new(default, false, false, errors);
}
