using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Validation;

namespace DotBased.AspNet.Authority.Validators;

public interface IPasswordValidator<TUser> where TUser : class
{
    public Task<ValidationResult> ValidatePasswordAsync(AuthorityUserManager<TUser> userManager, TUser user, string password);
}