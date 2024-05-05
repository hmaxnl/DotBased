namespace DotBased;

/// <summary>
/// Simple result class for returning a result state or a message and a exception.
/// </summary>
public class Result(bool success, string message, Exception? exception)
{
    public bool Success { get; set; } = success;
    public string Message { get; set; } = message;
    public Exception? Exception { get; set; } = exception;

    public static Result Ok() => new Result(true, string.Empty, null);
    public static Result Failed(string message, Exception? exception = null) => new Result(false, message, exception);
}

public class Result<TValue>(bool success, string message, TValue? value, Exception? exception) : Result(success, message, exception)
{
    public TValue? Value { get; set; } = value;

    public static Result<TValue> Ok(TValue value) => new Result<TValue>(true, string.Empty, value, null);

    public new static Result<TValue> Failed(string message, Exception? exception = null) =>
        new Result<TValue>(false, message, default, exception);
}

public class ListResult<TItem>(bool success, string message, int totalCount, IEnumerable<TItem>? items, Exception? exception) : Result(success, message, exception)
{
    public readonly IReadOnlyList<TItem> Items = items != null ? new List<TItem>(items) : new List<TItem>();
    /// <summary>
    /// The amount of items that this result contains.
    /// </summary>
    public int Count => Items.Count;

    /// <summary>
    /// The total amount of item that is available.
    /// </summary>
    public int TotalCount { get; } = totalCount;

    public static ListResult<TItem> Ok(IEnumerable<TItem> items, int totalCount = -1) =>
        new ListResult<TItem>(true, string.Empty, totalCount, items, null);

    public new static ListResult<TItem> Failed(string message, Exception? exception = null) =>
        new ListResult<TItem>(false, message, -1,null, exception);
}