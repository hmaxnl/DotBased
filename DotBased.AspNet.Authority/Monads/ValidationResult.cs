using DotBased.AspNet.Authority.Models.Validation;
using DotBased.Monads;

namespace DotBased.AspNet.Authority.Monads;

public class ValidationResult : Result
{
    private ValidationResult()
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
    
    
    public static implicit operator ValidationResult(Exception exception) => new(exception);
    public static implicit operator ValidationResult(ResultError error) => new(error);
    public static implicit operator ValidationResult(List<ValidationError> validationErrors) => new(validationErrors);

    public static ValidationResult FromResult(Result result)
    {
        var validationResult = result.Match<ValidationResult>(
            () => new ValidationResult(),
            error => new ValidationResult(error));
        return validationResult;
    }

    public new static ValidationResult Success() => new();
    public static ValidationResult Fail(List<ValidationError> validationErrors) => new(validationErrors);
}