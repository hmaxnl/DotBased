using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Models.Validation;
using ValidationResult = DotBased.AspNet.Authority.Monads.ValidationResult;

namespace DotBased.AspNet.Authority.Managers;

public partial class AuthorityManager
{
    public async Task<ValidationResult> ValidatePasswordAsync(AuthorityUser user, string password)
    {
        List<ValidationError> errors = [];
        foreach (var validator in PasswordValidators)
        {
            var validatorResult = await validator.ValidatePasswordAsync(this, user, password);
            if (!validatorResult.IsSuccess)
            {
                errors.AddRange(validatorResult.ValidationErrors);
            }
        }
        return errors.Count > 0 ? ValidationResult.Fail(errors) : ValidationResult.Success();
    }

    public async Task<ValidationResult> ValidateUserAsync(AuthorityUser user)
    {
        List<ValidationError> errors = [];
        foreach (var userValidator in UserValidators)
        {
            var validationResult = await userValidator.ValidateUserAsync(this, user);
            if (!validationResult.IsSuccess)
            {
                errors.AddRange(validationResult.ValidationErrors);
            }
        }
        return errors.Count > 0 ? ValidationResult.Fail(errors) : ValidationResult.Success();
    }

    public async Task<ListResultOld<AuthorityUserItem>> SearchUsersAsync(string query, int maxResults = 20, int offset = 0, CancellationToken cancellationToken = default)
    {
        var result = await UserRepository.GetAuthorityUsersAsync(maxResults, offset, query, cancellationToken);
        return result;
    }

    public async Task<AuthorityResultOldOld<AuthorityUser>> UpdatePasswordAsync(AuthorityUser user, string password, CancellationToken cancellationToken = default)
    {
        var passwordValidation = await ValidatePasswordAsync(user, password);
        if (!passwordValidation.IsSuccess)
        {
            return AuthorityResultOldOld<AuthorityUser>.Failed(passwordValidation.ValidationErrors, ResultFailReason.Validation);
        }

        user.PasswordHash = await PasswordHasher.HashPasswordAsync(password);
        user.SecurityVersion = GenerateVersion();

        var updateResult = await UserRepository.UpdateUserAsync(user, cancellationToken);
        return AuthorityResultOldOld<AuthorityUser>.FromResult(updateResult);
    }

    public async Task<AuthorityResultOldOld<AuthorityUser>> CreateUserAsync(AuthorityUser userModel, string password, CancellationToken cancellationToken = default)
    {
        var userValidation = await ValidateUserAsync(userModel);
        var passwordValidation = await ValidatePasswordAsync(userModel, password);
        if (!userValidation.IsSuccess || !passwordValidation.IsSuccess)
        {
            List<ValidationError> errors = [];
            errors.AddRange(userValidation.ValidationErrors);
            errors.AddRange(passwordValidation.ValidationErrors);
            return AuthorityResultOldOld<AuthorityUser>.Failed(errors, ResultFailReason.Validation);
        }
        
        userModel.Version = GenerateVersion();
        userModel.SecurityVersion = GenerateVersion();
        var hashedPassword = await PasswordHasher.HashPasswordAsync(password);
        userModel.PasswordHash = hashedPassword;

        var userCreationResult = await UserRepository.CreateUserAsync(userModel, cancellationToken);
        
        return AuthorityResultOldOld<AuthorityUser>.FromResult(userCreationResult);
    }

    public async Task<ResultOld<AuthorityUser>> UpdateUserAsync(AuthorityUser model, CancellationToken cancellationToken = default)
    {
        var updateResult = await UserRepository.UpdateUserAsync(model, cancellationToken);
        return updateResult;
    }

    public async Task<ResultOld> DeleteUserAsync(AuthorityUser model, CancellationToken cancellationToken = default)
    {
        var deleteResult = await UserRepository.DeleteUserAsync(model, cancellationToken);
        return deleteResult;
    }

    public async Task<ResultOld> IsValidUserAsync(AuthorityUser user, CancellationToken cancellationToken = default)
    {
        var usrResult = await UserRepository.GetVersionAsync(user, cancellationToken);
        return usrResult;
    }
}