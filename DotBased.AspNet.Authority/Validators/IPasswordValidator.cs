using DotBased.AspNet.Authority.Models.Validation;
using DotBased.AspNet.Authority.Services;

namespace DotBased.AspNet.Authority.Validators;

public interface IPasswordValidator<TUser> where TUser : class
{
    public Task<ValidationResult> ValidatePasswordAsync(AuthorityUserManager<TUser> userManager, TUser user, string password);
}