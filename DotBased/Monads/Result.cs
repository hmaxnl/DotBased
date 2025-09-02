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
        Error = ResultError.Error(exception);
    }

    protected Result(ResultError error)
    {
        IsSuccess = false;
        Error = error;
    }

    public bool IsSuccess { get; }
    public ResultError? Error { get; set; }
    
    public static implicit operator Result(Exception exception) => new(exception);
    public static implicit operator Result(ResultError error) => new(error);

    public static Result Success() => new();
    public static Result Fail(ResultError error) => new(error);
    public static Result Exception(Exception exception) => new(exception);
    
    
    public TMatch Match<TMatch>(Func<TMatch> success, Func<ResultError, TMatch> failure) => IsSuccess ? success() : failure(Error!);

    public void Match(Action success, Action<ResultError> failure)
    {
        if (IsSuccess)
        {
            success();
        }
        else
        {
            failure(Error!);
        }
    }
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
    
    protected Result(ResultError error) : base(error)
    {
        _result = default;
    }

    private readonly TResult? _result;
    public TResult Value => IsSuccess ? _result! : throw new InvalidOperationException("Result is invalid");

    public static implicit operator Result<TResult>(TResult result) => new(result);
    public static implicit operator Result<TResult>(Exception exception) => new(exception);
    public static implicit operator Result<TResult>(ResultError error) => new(error);
    
    public static Result<TResult> Success(TResult result) => new(result);
    public new static Result<TResult> Fail(ResultError error) => new(error);
    public new static Result<TResult> Exception(Exception exception) => new(exception);
    
    public TMatch Match<TMatch>(Func<TResult, TMatch> success, Func<ResultError, TMatch> failure) => 
        IsSuccess && Value != null ? success(Value) : failure(Error ?? ResultError.Fail("No error and value is null!"));
}

public class ResultError
{
    private ResultError(string description, Exception? exception)
    {
        Description = description;
        Exception = exception;
    }

    public string Description { get; }
    public Exception? Exception { get; }

    public static ResultError Fail(string description) => new(description, null);
    public static ResultError Error(Exception exception, string description = "") => new(description, exception);
}