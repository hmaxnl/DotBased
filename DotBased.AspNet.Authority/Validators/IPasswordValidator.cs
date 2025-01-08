using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Models.Validation;

namespace DotBased.AspNet.Authority.Validators;

public interface IPasswordValidator
{
    public Task<ValidationResult> ValidatePasswordAsync(AuthorityUserManager userManager, AuthorityUser user, string password);
}