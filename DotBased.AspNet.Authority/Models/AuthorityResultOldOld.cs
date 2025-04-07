using DotBased.AspNet.Authority.Models.Validation;

namespace DotBased.AspNet.Authority.Models;

public class AuthorityResultOldOld<TResultValue> : ResultOld<TResultValue>
{
    public static AuthorityResultOldOld<TResultValue> FromResult(ResultOld<TResultValue> resultOld) => new AuthorityResultOldOld<TResultValue>(resultOld);
    
    public AuthorityResultOldOld(ResultOld<TResultValue> resultOld) : base(resultOld)
    {
        Reason = ResultFailReason.Unknown;
    }
    
    public AuthorityResultOldOld(bool success, string errorMessage = "", TResultValue? value = default, ResultFailReason reason = ResultFailReason.None, IReadOnlyList<ValidationError>? errors = null) : base(success, errorMessage, value, null)
    {
        Success = success;
        Message = errorMessage;
        Value = value;
        Reason = reason;
        ValidationErrors = errors;
    }
    public ResultFailReason Reason { get; }
    public IReadOnlyList<ValidationError>? ValidationErrors { get; }
    

    public new static AuthorityResultOldOld<TResultValue> Ok(TResultValue? value) => new AuthorityResultOldOld<TResultValue>(true, value:value);

    public static AuthorityResultOldOld<TResultValue> Error(string errorMessage, ResultFailReason reason = ResultFailReason.Error) =>
        new AuthorityResultOldOld<TResultValue>(false, errorMessage, reason:reason);

    public static AuthorityResultOldOld<TResultValue> Failed(IReadOnlyList<ValidationError> errors, ResultFailReason reason = ResultFailReason.None)
        => new AuthorityResultOldOld<TResultValue>(false, errors:errors, reason:reason);
}

public enum ResultFailReason
{
    None,
    Unknown,
    Validation,
    Error
}