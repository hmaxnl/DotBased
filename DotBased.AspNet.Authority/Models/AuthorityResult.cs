using DotBased.AspNet.Authority.Models.Validation;

namespace DotBased.AspNet.Authority.Models;

public class AuthorityResult<TResultValue>
{
    public AuthorityResult(bool success, string errorMessage = "", TResultValue? value = default, ResultFailReason reason = ResultFailReason.None, List<ValidationError>? errors = null)
    {
        Success = success;
        ErrorMessage = errorMessage;
        Value = value;
        Reason = reason;
        ValidationErrors = errors;
    }
    
    public bool Success { get; }
    public string ErrorMessage { get; }
    public TResultValue? Value { get; }
    public ResultFailReason Reason { get; }
    public List<ValidationError>? ValidationErrors { get; }
    

    public static AuthorityResult<TResultValue> Ok(TResultValue? value) => new AuthorityResult<TResultValue>(true, value:value);

    public static AuthorityResult<TResultValue> Error(string errorMessage, ResultFailReason reason = ResultFailReason.Error) =>
        new AuthorityResult<TResultValue>(false, errorMessage, reason:reason);

    public static AuthorityResult<TResultValue> Failed(List<ValidationError> errors, ResultFailReason reason = ResultFailReason.None)
        => new AuthorityResult<TResultValue>(false, errors:errors, reason:reason);
}

public enum ResultFailReason
{
    None,
    Unknown,
    Validation,
    Error
}