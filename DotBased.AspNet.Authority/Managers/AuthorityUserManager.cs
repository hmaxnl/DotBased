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

    public async Task<ListResult<AuthorityUser>> SearchUsersAsync(string query, int maxResults = 20, int offset = 0, CancellationToken? cancellationToken = null)
    {
        var searchResult = await UserRepository.GetAuthorityUsersAsync(query, maxResults, offset, cancellationToken);
        return searchResult.Item1 == null ? ListResult<AuthorityUser>.Failed("No results!") : ListResult<AuthorityUser>.Ok(searchResult.Item1, searchResult.Item2);
    }

    public async Task<AuthorityResult<AuthorityUser>> UpdatePasswordAsync(AuthorityUser user, string password, CancellationToken? cancellationToken = null)
    {
        var passwordValidation = await ValidatePasswordAsync(user, password);
        if (!passwordValidation.Success)
        {
            List<ValidationError> errors = [];
            errors.AddRange(passwordValidation.Errors);
            return AuthorityResult<AuthorityUser>.Failed(errors, ResultFailReason.Validation);
        }

        user.PasswordHash = await PasswordHasher.HashPasswordAsync(password);
        user.SecurityVersion = GenerateVersion();

        var updateResult = await UserRepository.UpdateUserAsync(user, cancellationToken);
        return updateResult == null ? AuthorityResult<AuthorityUser>.Error("Failed to save updates!") : AuthorityResult<AuthorityUser>.Ok(updateResult);
    }

    public async Task<AuthorityResult<AuthorityUser>> CreateUserAsync(AuthorityUser userModel, string password, CancellationToken? cancellationToken = null)
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

        return userCreationResult != null
            ? AuthorityResult<AuthorityUser>.Ok(userCreationResult)
            : AuthorityResult<AuthorityUser>.Error("Failed to create user in repository!");
    }

    public async Task<Result<AuthorityUser>> UpdateUserAsync(AuthorityUser model, CancellationToken? cancellationToken = null)
    {
        var updateResult = await UserRepository.UpdateUserAsync(model, cancellationToken);
        return updateResult != null ? Result<AuthorityUser>.Ok(updateResult) : Result<AuthorityUser>.Failed("Failed to update user in repository!");
    }

    public async Task<bool> DeleteUserAsync(AuthorityUser model, CancellationToken? cancellationToken = null)
    {
        var deleteResult = await UserRepository.DeleteUserAsync(model, cancellationToken);
        return deleteResult;
    }
    
    
}