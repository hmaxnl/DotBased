using DotBased.AspNet.Authority.Validators;
using DotBased.Logging;

namespace DotBased.AspNet.Authority.Services;

public class AuthorityUserManager<TUser>
{
    public AuthorityUserManager(
        AuthorityManager manager,
        IEnumerable<IPasswordValidator<TUser>>? passwordValidators,
        IEnumerable<IUserValidator<TUser>>? userValidators)
    {
        _logger = LogService.RegisterLogger<AuthorityUserManager<TUser>>();
        AuthorityManager = manager;
        if (passwordValidators != null)
            PasswordValidators = passwordValidators;
        if (userValidators != null)
            UserValidators = userValidators;
    }

    private readonly ILogger _logger;
    public AuthorityManager AuthorityManager { get; }
    
    public IEnumerable<IPasswordValidator<TUser>> PasswordValidators { get; } = [];
    public IEnumerable<IUserValidator<TUser>> UserValidators { get; } = [];
    
    
}