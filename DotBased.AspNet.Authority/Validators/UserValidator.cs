using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Authority;
using DotBased.AspNet.Authority.Models.Options;
using DotBased.AspNet.Authority.Models.Validation;

namespace DotBased.AspNet.Authority.Validators;

public class UserValidator : IUserValidator
{
    private const string ValidatorId = "Authority.Validator.User";
    private const string ValidationBase = "Authority.Validation.User";

    public async Task<ValidationResult> ValidateUserAsync(AuthorityManager manager, AuthorityUser user)
    {
        List<ValidationError> errors = [];

        var userOptions = manager.Options.User;

        if (userOptions.RequireUniqueEmail)
        {
            if (string.IsNullOrWhiteSpace(user.EmailAddress))
            {
                errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.NoEmail",
                    $"Option {nameof(UserOptions.RequireUniqueEmail)} is set to true but given user does not have an email address!"));
            }
            else
            {
                var userEmailResult = await manager.UserRepository.GetAuthorityUserByEmailAsync(user.EmailAddress);
                if (userEmailResult != null)
                {
                    errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.EmailExists",
                        "Given email has already registered an account!"));
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(user.UserName))
        {
            if (userOptions.UserNameBlackList.Count != 0 && userOptions.UserNameBlackList.Contains(user.UserName, userOptions.UserNameBlackListComparer))
            {
                errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.Blacklisted", "Given username is not allowed (blacklisted)"));
            }

            if (!string.IsNullOrWhiteSpace(userOptions.UserNameCharacters))
            {
                List<char> chars = [];
                if (userOptions.UserNameCharacterListType == ListOption.Whitelist)
                {
                    chars.AddRange(user.UserName.Where(userNameChar => !userOptions.UserNameCharacters.Contains(userNameChar)));
                }
                if (userOptions.UserNameCharacterListType == ListOption.Blacklist)
                {
                    chars.AddRange(user.UserName.Where(userNameChar => userOptions.UserNameCharacters.Contains(userNameChar)));
                }

                if (chars.Count <= 0) return errors.Count > 0 ? ValidationResult.Failed(errors) : ValidationResult.Ok();
                var errorCode = "";
                var description = "";
                switch (userOptions.UserNameCharacterListType)
                {
                    case ListOption.Whitelist:
                        errorCode = "CharactersNotOnWhitelist";
                        description = $"Found characters in username that were not on the whitelist! Chars: [{string.Join(',', chars)}]";
                        break;
                    case ListOption.Blacklist:
                        errorCode = "CharactersOnBlacklist";
                        description = $"Found characters in username that are on the blacklist! Chars: [{string.Join(',', chars)}]";
                        break;
                }

                errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.UserName.{errorCode}", description));
            }
        }
        else
        {
            errors.Add(new ValidationError(ValidatorId, $"{ValidationBase}.InvalidUserName", "No username given!"));
        }

        return errors.Count > 0 ? ValidationResult.Failed(errors) : ValidationResult.Ok();
    }
}