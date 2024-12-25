using DotBased.AspNet.Authority.Models.Validation;
using DotBased.AspNet.Authority.Services;
using DotBased.Extensions;

namespace DotBased.AspNet.Authority.Validators;

/// <summary>
/// Validates the password against the options that is configured.
/// </summary>
/// <typeparam name="TUser">The user model used.</typeparam>
public class PasswordOptionsValidator<TUser> : IPasswordValidator<TUser>
{
    private const string ValidatorId = "Authority.Validator.Password.Options";
    private const string ValidationBase = "Authority.Validation.Password";
    
    public async Task<ValidationResult> ValidatePasswordAsync(AuthorityUserManager<TUser> userManager, string password)
    {
        if (userManager == null)
        {
            throw new ArgumentNullException(nameof(userManager), "User manager is not provided!");
        }
        var passwordOptions = userManager.AuthorityManager.Options.Password;
        var errors = new List<ValidationError>();
        
        if (password.IsNullOrEmpty() || password.Length < passwordOptions.RequiredLength)
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.Required.Length", $"Password needs to have a minimum length of {passwordOptions.RequiredLength}"));
        }

        if (passwordOptions.RequireDigit && !ContainsDigit(password))
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.Required.Digit", "Password must contain a digit!"));
        }

        if (passwordOptions.RequireNonAlphanumeric && ContainsNonAlphanumeric(password))
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.Required.NonAlphanumeric", "Password must contain a non alphanumeric character."));
        }

        if (passwordOptions.RequireLowercase && password.Any(char.IsLower))
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.Required.Lowercase", "Password must contains at least one lowercase character."));
        }

        if (passwordOptions.RequireUppercase && password.Any(char.IsUpper))
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.Required.Uppercase", "Password must contains at least one uppercase character."));
        }
        
        if (passwordOptions.PasswordBlackList.Count != 0 && passwordOptions.PasswordBlackList.Contains(password, passwordOptions.PasswordBlackListComparer))
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.Blacklisted", "Given password is not allowed (blacklisted)"));
        }

        if (passwordOptions.MinimalUniqueChars > 0 && password.Distinct().Count() < passwordOptions.MinimalUniqueChars)
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.UniqueChars", $"Password must contain at least {passwordOptions.MinimalUniqueChars} unique chars."));
        }

        return await Task.FromResult(errors.Count > 0 ? ValidationResult.Failed(errors) : ValidationResult.Ok());
    }

    private bool ContainsDigit(string strVal) => strVal.Any(char.IsDigit);

    private bool ContainsNonAlphanumeric(string strVal) => !strVal.Any(char.IsLetterOrDigit);
}