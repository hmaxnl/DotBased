using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Validation;

namespace DotBased.AspNet.Authority.Validators;

public interface IUserValidator<TUser> where TUser : class
{
    public Task<ValidationResult> ValidateUserAsync(AuthorityUserManager<TUser> manager, TUser user);
}