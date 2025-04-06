namespace DotBased.Monads;

public class Result
{
    protected Result()
    {
        IsSuccess = true;
    }

    protected Result(Exception exception)
    {
        IsSuccess = false;
        Information = ResultInformation.Error(exception);
    }

    protected Result(ResultInformation information)
    {
        IsSuccess = false;
        Information = information;
    }

    public bool IsSuccess { get; }
    public ResultInformation? Information { get; set; }
    
    public static implicit operator Result(Exception exception) => new(exception);
    public static implicit operator Result(ResultInformation information) => new(information);

    public static Result Success() => new();
    public static Result Error(ResultInformation information) => new(information);
    public static Result Fail(Exception exception) => new(exception);
    
    
    public TMatch Match<TMatch>(Func<TMatch> success, Func<ResultInformation, TMatch> failure) => IsSuccess ? success() : failure(Information!);
}

public class Result<TResult> : Result
{
    protected Result(TResult result)
    {
        _result = result;
    }
    
    protected Result(Exception exception) : base(exception)
    {
        _result = default;
    }
    
    protected Result(ResultInformation information) : base(information)
    {
        _result = default;
    }

    private readonly TResult? _result;
    public TResult Value => IsSuccess ? _result! : throw new InvalidOperationException("Result is invalid");

    public static implicit operator Result<TResult>(TResult result) => new(result);
    public static implicit operator Result<TResult>(Exception exception) => new(exception);
    public static implicit operator Result<TResult>(ResultInformation information) => new(information);
    
    public static Result<TResult> Success(TResult result) => new(result);
    public new static Result<TResult> Error(ResultInformation information) => new(information);
    public new static Result<TResult> Fail(Exception exception) => new(exception);
    
    public TMatch Match<TMatch>(Func<TResult, TMatch> success, Func<ResultInformation, TMatch> failure)
    {
        return IsSuccess && _result != null ? success(_result) : failure(Information ?? ResultInformation.Fail("No error and value is null!"));
    }
}

public class ResultInformation
{
    private ResultInformation(string message, Exception? exception)
    {
        Message = message;
        Exception = exception;
    }

    public string Message { get; }
    public Exception? Exception { get; }

    public static ResultInformation Info(string message) => new(message, null);
    public static ResultInformation Fail(string message) => new(message, null);
    public static ResultInformation Error(Exception exception, string message = "") => new(message, exception);
}