namespace DotBased.AspNet.Authority.Models.Validation;

public class ValidationResult
{
    public ValidationResult(bool success, IEnumerable<ValidationError>? errors = null)
    {
        if (errors != null)
        {
            Errors = errors.ToList();
        }
        Success = success;
    }
    
    public bool Success { get; }
    public IReadOnlyList<ValidationError> Errors { get; } = [];

    public static ValidationResult Failed(IEnumerable<ValidationError> errors) => new(false, errors);
    public static ValidationResult Ok() => new(true);

    public override string ToString() => Success ? "Success" : $"Failed ({Errors.Count} errors)";
}