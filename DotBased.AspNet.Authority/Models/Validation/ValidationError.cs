namespace DotBased.AspNet.Authority.Models.Validation;

public class ValidationError
{
    public ValidationError(string validator, string errorCode, string description)
    {
        Validator = validator;
        ErrorCode = errorCode;
        Description = description;
    }
    
    /// <summary>
    /// The validator name that generated this error.
    /// </summary>
    public string Validator { get; }
    /// <summary>
    /// The error code
    /// </summary>
    public string ErrorCode { get; }
    /// <summary>
    /// Error description
    /// </summary>
    public string Description { get; }
}