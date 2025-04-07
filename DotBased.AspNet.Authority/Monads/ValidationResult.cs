using DotBased.AspNet.Authority.Models.Validation;
using DotBased.Monads;

namespace DotBased.AspNet.Authority.Monads;

public class ValidationResult<T> : Result<T>
{
    private ValidationResult(T result) : base(result)
    {
    }

    private ValidationResult(Exception exception) : base(exception)
    {
    }

    private ValidationResult(ResultError error) : base(error)
    {
    }

    private ValidationResult(List<ValidationError> validationErrors) : base(ResultError.Fail("Validation failed!"))
    {
        _validationErrors = validationErrors;
    }

    private readonly List<ValidationError> _validationErrors = [];
    public IReadOnlyList<ValidationError> ValidationErrors => _validationErrors;
    
    public static implicit operator ValidationResult<T>(T result) => new(result);
    public static implicit operator ValidationResult<T>(Exception exception) => new(exception);
    public static implicit operator ValidationResult<T>(ResultError error) => new(error);
    public static implicit operator ValidationResult<T>(List<ValidationError> validationErrors) => new(validationErrors);

    public static ValidationResult<T> FromResult(Result<T> result)
    {
        var authorityResult = result.Match<ValidationResult<T>>(
            r => new ValidationResult<T>(r),
            error => new ValidationResult<T>(error));
        return authorityResult;
    }

    public TMatch Match<TMatch>(Func<T, TMatch> success, Func<ResultError, IReadOnlyList<ValidationError>, TMatch> failure) =>
        IsSuccess && Value != null ? success(Value) : failure(Error ?? ResultError.Fail("No error and value is null!"), ValidationErrors);
}