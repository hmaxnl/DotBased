using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Monads;

namespace DotBased.AspNet.Authority.Validators;

public interface IUserValidator
{
    public Task<ValidationResult> ValidateUserAsync(AuthorityManager manager, AuthorityUser user);
}