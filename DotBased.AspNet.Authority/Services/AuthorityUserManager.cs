using DotBased.AspNet.Authority.Crypto;
using DotBased.AspNet.Authority.Models.Validation;
using DotBased.AspNet.Authority.Repositories;
using DotBased.AspNet.Authority.Validators;
using DotBased.Logging;

namespace DotBased.AspNet.Authority.Services;

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
}