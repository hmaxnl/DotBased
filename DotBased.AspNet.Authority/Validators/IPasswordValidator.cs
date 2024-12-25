using DotBased.AspNet.Authority.Models.Validation;
using DotBased.AspNet.Authority.Services;

namespace DotBased.AspNet.Authority.Validators;

public interface IPasswordValidator<TUser>
{
    public Task<ValidationResult> ValidatePasswordAsync(AuthorityUserManager<TUser> userManager, string password);
}