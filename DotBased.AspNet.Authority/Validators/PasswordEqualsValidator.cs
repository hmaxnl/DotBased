using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Models.Validation;
using DotBased.AspNet.Authority.Monads;

namespace DotBased.AspNet.Authority.Validators;

public class PasswordEqualsValidator : IPasswordValidator
{
    private const string ValidatorId = "Authority.Validator.Password.Equals";
    private const string ValidationBase = "Authority.Validation.Password";
    public async Task<ValidationResult> ValidatePasswordAsync(AuthorityManager userManager, AuthorityUser user, string password)
    {
        List<ValidationError> errors = [];
        var hashedPassword = await userManager.PasswordHasher.HashPasswordAsync(password);
        if (user.PasswordHash != null && user.PasswordHash.Equals(hashedPassword, StringComparison.Ordinal))
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.InUse", "User uses this password already!"));
        }

        return errors.Count > 0 ? ValidationResult.Fail(errors) : ValidationResult.Success();
    }
}