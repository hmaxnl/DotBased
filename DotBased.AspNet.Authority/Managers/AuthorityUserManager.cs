using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Models.Validation;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager
{
    public async Task<ValidationResult> ValidatePasswordAsync(AuthorityUser user, string password)
    {
        List<ValidationError> errors = [];
        foreach (var validator in PasswordValidators)
        {
            var validatorResult = await validator.ValidatePasswordAsync(this, user, password);
            if (!validatorResult.Success)
            {
                errors.AddRange(validatorResult.Errors);
            }
        }
        return errors.Count > 0 ? ValidationResult.Failed(errors) : ValidationResult.Ok();
    }

    public async Task<ValidationResult> ValidateUserAsync(AuthorityUser user)
    {
        List<ValidationError> errors = [];
        foreach (var userValidator in UserValidators)
        {
            var validationResult = await userValidator.ValidateUserAsync(this, user);
            if (!validationResult.Success)
            {
                errors.AddRange(validationResult.Errors);
            }
        }
        return errors.Count > 0 ? ValidationResult.Failed(errors) : ValidationResult.Ok();
    }

    public async Task<ListResult<AuthorityUserItem>> SearchUsersAsync(string query, int maxResults = 20, int offset = 0, CancellationToken cancellationToken = default)
    {
        var result = await UserRepository.GetAuthorityUsersAsync(maxResults, offset, query, cancellationToken);
        return result;
    }

    public async Task<AuthorityResult<AuthorityUser>> UpdatePasswordAsync(AuthorityUser user, string password, CancellationToken cancellationToken = default)
    {
        var passwordValidation = await ValidatePasswordAsync(user, password);
        if (!passwordValidation.Success)
        {
            return AuthorityResult<AuthorityUser>.Failed(passwordValidation.Errors, ResultFailReason.Validation);
        }

        user.PasswordHash = await PasswordHasher.HashPasswordAsync(password);
        user.SecurityVersion = GenerateVersion();

        var updateResult = await UserRepository.UpdateUserAsync(user, cancellationToken);
        return AuthorityResult<AuthorityUser>.FromResult(updateResult);
    }

    public async Task<AuthorityResult<AuthorityUser>> CreateUserAsync(AuthorityUser userModel, string password, CancellationToken cancellationToken = default)
    {
        var userValidation = await ValidateUserAsync(userModel);
        var passwordValidation = await ValidatePasswordAsync(userModel, password);
        if (!userValidation.Success || !passwordValidation.Success)
        {
            List<ValidationError> errors = [];
            errors.AddRange(userValidation.Errors);
            errors.AddRange(passwordValidation.Errors);
            return AuthorityResult<AuthorityUser>.Failed(errors, ResultFailReason.Validation);
        }
        
        userModel.Version = GenerateVersion();
        userModel.SecurityVersion = GenerateVersion();
        var hashedPassword = await PasswordHasher.HashPasswordAsync(password);
        userModel.PasswordHash = hashedPassword;

        var userCreationResult = await UserRepository.CreateUserAsync(userModel, cancellationToken);
        
        return AuthorityResult<AuthorityUser>.FromResult(userCreationResult);
    }

    public async Task<Result<AuthorityUser>> UpdateUserAsync(AuthorityUser model, CancellationToken cancellationToken = default)
    {
        var updateResult = await UserRepository.UpdateUserAsync(model, cancellationToken);
        return updateResult;
    }

    public async Task<Result> DeleteUserAsync(AuthorityUser model, CancellationToken cancellationToken = default)
    {
        var deleteResult = await UserRepository.DeleteUserAsync(model, cancellationToken);
        return deleteResult;
    }

    public async Task<Result> IsValidUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        var usrResult = await userRepository.GetVersionAsync(user, cancellationToken);
        return usrResult;
    }
}