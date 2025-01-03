using DotBased.AspNet.Authority.Crypto;
using DotBased.AspNet.Authority.Models;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Models.Validation;
using DotBased.AspNet.Authority.Repositories;
using DotBased.AspNet.Authority.Validators;
using DotBased.Logging;

namespace DotBased.AspNet.Authority.Managers;

public class AuthorityUserManager<TUser> where TUser : class
{
    public AuthorityUserManager(
        AuthorityManager manager,
        IUserRepository<TUser> userRepository,
        IPasswordHasher passwordHasher,
        IEnumerable<IPasswordValidator<TUser>>? passwordValidators,
        IEnumerable<IUserValidator<TUser>>? userValidators)
    {
        _logger = LogService.RegisterLogger<AuthorityUserManager<TUser>>();
        AuthorityManager = manager;
        UserRepository = userRepository;
        PasswordHasher = passwordHasher;
        if (passwordValidators != null)
            PasswordValidators = passwordValidators;
        if (userValidators != null)
            UserValidators = userValidators;
    }

    private readonly ILogger _logger;
    public AuthorityManager AuthorityManager { get; }
    public IUserRepository<TUser> UserRepository { get; }
    
    public IPasswordHasher PasswordHasher { get; }
    
    public IEnumerable<IPasswordValidator<TUser>> PasswordValidators { get; } = [];
    public IEnumerable<IUserValidator<TUser>> UserValidators { get; } = [];

    public async Task<ValidationResult> ValidatePasswordAsync(TUser user, string password)
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

    public async Task<ValidationResult> ValidateUserAsync(TUser user)
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

    public async Task<ListResult<TUser>> SearchUsersAsync(string query, int maxResults = 20, int offset = 0)
    {
        var searchResult = await UserRepository.GetUsersAsync(query, maxResults, offset);
        return searchResult.Item1 == null ? ListResult<TUser>.Failed("No results!") : ListResult<TUser>.Ok(searchResult.Item1, searchResult.Item2);
    }

    public async Task<AuthorityResult<TUser>> UpdatePasswordAsync(TUser user, string password)
    {
        if (user is not AuthorityUserBase userBase)
        {
            return AuthorityResult<TUser>.Error($"Given user is not of base type {nameof(AuthorityUserBase)}!");
        }
        
        var passwordValidation = await ValidatePasswordAsync(user, password);
        if (!passwordValidation.Success)
        {
            List<ValidationError> errors = [];
            errors.AddRange(passwordValidation.Errors);
            return AuthorityResult<TUser>.Failed(errors, ResultFailReason.Validation);
        }

        userBase.PasswordHash = await PasswordHasher.HashPasswordAsync(password);
        userBase.SecurityVersion = AuthorityManager.GenerateVersion();

        var updateResult = await UserRepository.UpdateUserAsync(user);
        return updateResult == null ? AuthorityResult<TUser>.Error("Failed to save updates!") : AuthorityResult<TUser>.Ok(updateResult);
    }

    public async Task<AuthorityResult<TUser>> CreateUserAsync(TUser userModel, string password)
    {
        if (userModel is not AuthorityUserBase userBase)
        {
            return AuthorityResult<TUser>.Error($"Given user is not of base type {nameof(AuthorityUserBase)}!");
        }

        var userValidation = await ValidateUserAsync(userModel);
        var passwordValidation = await ValidatePasswordAsync(userModel, password);
        if (!userValidation.Success || !passwordValidation.Success)
        {
            List<ValidationError> errors = [];
            errors.AddRange(userValidation.Errors);
            errors.AddRange(passwordValidation.Errors);
            return AuthorityResult<TUser>.Failed(errors, ResultFailReason.Validation);
        }
        
        userBase.Version = AuthorityManager.GenerateVersion();
        userBase.SecurityVersion = AuthorityManager.GenerateVersion();
        var hashedPassword = await PasswordHasher.HashPasswordAsync(password);
        userBase.PasswordHash = hashedPassword;

        var userCreationResult = await UserRepository.CreateUserAsync(userModel);

        return userCreationResult != null
            ? AuthorityResult<TUser>.Ok(userCreationResult)
            : AuthorityResult<TUser>.Error("Failed to create user in repository!");
    }

    public async Task<Result<TUser>> UpdateUserAsync(TUser model)
    {
        var updateResult = await UserRepository.UpdateUserAsync(model);
        return updateResult != null ? Result<TUser>.Ok(updateResult) : Result<TUser>.Failed("Failed to update user in repository!");
    }

    public async Task<bool> DeleteUserAsync(TUser model)
    {
        var deleteResult = await UserRepository.DeleteUserAsync(model);
        return deleteResult;
    }
    
    
}