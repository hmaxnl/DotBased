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
        
        var version = AuthorityManager.GenerateVersion();
        userBase.Version = version;
        var securityVersion = AuthorityManager.GenerateVersion();
        userBase.SecurityVersion = securityVersion;
        var hashedPassword = await PasswordHasher.HashPasswordAsync(password);
        userBase.PasswordHash = hashedPassword;

        var userCreationResult = await UserRepository.CreateUserAsync(userModel);

        return userCreationResult != null
            ? AuthorityResult<TUser>.Ok(userCreationResult)
            : AuthorityResult<TUser>.Error("Failed to create user in repository!");
    }
}