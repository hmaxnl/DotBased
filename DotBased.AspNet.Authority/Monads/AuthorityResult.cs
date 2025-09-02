using DotBased.AspNet.Authority.Models.Validation;
using DotBased.Monads;

namespace DotBased.AspNet.Authority.Monads;

public class AuthorityResult<TResult> : Result<TResult>
{
    protected AuthorityResult(TResult result) : base(result)
    {
    }

    protected AuthorityResult(Exception exception) : base(exception)
    {
    }

    protected AuthorityResult(ResultError error) : base(error)
    {
    }

    protected AuthorityResult(List<ValidationError> validationErrors) : base(ResultError.Fail("Validation failed!"))
    {
        _validationErrors = validationErrors;
    }

    private readonly List<ValidationError> _validationErrors = [];
    public IReadOnlyList<ValidationError> ValidationErrors => _validationErrors;

    public static implicit operator AuthorityResult<TResult>(TResult result) => new(result);
    public static implicit operator AuthorityResult<TResult>(Exception exception) => new(exception);
    public static implicit operator AuthorityResult<TResult>(ResultError error) => new(error);
    public static implicit operator AuthorityResult<TResult>(List<ValidationError> validationErrors) => new(validationErrors);

    public static AuthorityResult<TResult> FromResult(Result<TResult> result)
    {
        var authorityResult = result.Match<AuthorityResult<TResult>>(
            r => new AuthorityResult<TResult>(r),
            error => new AuthorityResult<TResult>(error));
        return authorityResult;
    }
}