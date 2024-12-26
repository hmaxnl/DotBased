using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Models.Validation;
using DotBased.AspNet.Authority.Services;

namespace DotBased.AspNet.Authority.Validators;

public class PasswordEqualsValidator<TUser> : IPasswordValidator<TUser> where TUser : class
{
    private const string ValidatorId = "Authority.Validator.Password.Equals";
    private const string ValidationBase = "Authority.Validation.Password";
    public async Task<ValidationResult> ValidatePasswordAsync(AuthorityUserManager<TUser> userManager, TUser user, string password)
    {
        if (user == null || user is not AuthorityUserBase authorityUser)
        {
            throw new ArgumentException("Invalid user given!", nameof(user));
        }

        List<ValidationError> errors = [];
        var hashedPassword = await userManager.PasswordHasher.HashPasswordAsync(password);
        if (authorityUser.PasswordHash != null && authorityUser.PasswordHash.Equals(hashedPassword, StringComparison.Ordinal))
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.InUse", "User uses this password already!"));
        }

        return errors.Count > 0 ? ValidationResult.Failed(errors) : ValidationResult.Ok();
    }
}