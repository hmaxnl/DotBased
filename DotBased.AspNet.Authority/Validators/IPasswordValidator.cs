using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Monads;

namespace DotBased.AspNet.Authority.Validators;

public interface IPasswordValidator
{
    public Task<ValidationResult> ValidatePasswordAsync(AuthorityManager manager, AuthorityUser user, string password);
}