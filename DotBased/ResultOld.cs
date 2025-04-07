namespace DotBased;

/// <summary>
/// Simple result class for returning a result state or a message and an exception.
/// </summary>
public class ResultOld
{
    public ResultOld(bool success, string message, Exception? exception)
    {
        Success = success;
        Message = message;
        Exception = exception;
    }

    public ResultOld(ResultOld bObj)
    {
        Success = bObj.Success;
        Message = bObj.Message;
        Exception = bObj.Exception;
    }
    
    public bool Success { get; set; }
    public string Message { get; set; }
    public Exception? Exception { get; set; }

    public static ResultOld Ok() => new(true, string.Empty, null);
    public static ResultOld Failed(string message, Exception? exception = null) => new(false, message, exception);
}

public class ResultOld<TValue> : ResultOld
{
    public ResultOld(bool success, string message, TValue? value, Exception? exception) : base(success, message, exception)
    {
        Value = value;
    }
    public ResultOld(ResultOld bObj) : base(bObj)
    {
        
    }
    public TValue? Value { get; set; }

    public static ResultOld<TValue> Ok(TValue value) => new(true, string.Empty, value, null);

    public new static ResultOld<TValue> Failed(string message, Exception? exception = null) =>
        new(false, message, default, exception);

    public static ResultOld<TValue> HandleResult(TValue? value, string failedMessage, Exception? exception = null)
    {
        return value == null ? Failed(failedMessage, exception) : Ok(value);
    }
}

public class ListResultOld<TItem> : ResultOld
{
    public ListResultOld(bool success, string message, int totalCount, IEnumerable<TItem>? items, int limit = -1, int offset = -1, Exception? exception = null) : base(success, message, exception)
    {
        Items = items != null ? new List<TItem>(items) : new List<TItem>();
        TotalCount = totalCount;
        Limit = limit;
        Offset = offset;
    }

    public ListResultOld(ResultOld bObj) : base(bObj)
    {
        Items = new List<TItem>();
    }
    
    public readonly IReadOnlyList<TItem> Items;
    /// <summary>
    /// The amount of items that this result contains.
    /// </summary>
    public int Count => Items.Count;

    /// <summary>
    /// The total amount of item that is available.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// The limit this result contains
    /// </summary>
    public int Limit { get; }
    
    /// <summary>
    /// The offset this result has the items from.
    /// </summary>
    public int Offset { get; }

    public static ListResultOld<TItem> Ok(IEnumerable<TItem> items, int totalCount = -1, int limit = -1, int offset = -1) =>
        new(true, string.Empty, totalCount, items, limit, offset);

    public new static ListResultOld<TItem> Failed(string message, Exception? exception = null) =>
        new(false, message, -1, null, exception: exception);
}