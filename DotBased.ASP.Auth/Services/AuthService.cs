using DotBased.ASP.Auth.Domains;
using DotBased.ASP.Auth.Domains.Auth;
using DotBased.ASP.Auth.Domains.Identity;
using DotBased.Extensions;
using DotBased.Logging;

namespace DotBased.ASP.Auth.Services;

public class AuthService
{
    public AuthService(IAuthDataRepository authDataRepository, AuthDataCache dataCache)
    {
        _authDataRepository = authDataRepository;
        _dataCache = dataCache;
        _logger = LogService.RegisterLogger(typeof(AuthService));
    }

    private readonly IAuthDataRepository _authDataRepository;
    private readonly AuthDataCache _dataCache;
    private readonly ILogger _logger;

    public async Task<Result<AuthenticationStateModel>> LoginAsync(LoginModel login)
    {
        UserModel? user = null;
        Result<UserModel> usrResult;
        if (!login.UserName.IsNullOrWhiteSpace())
        {
            usrResult = await _authDataRepository.GetUserAsync(string.Empty, string.Empty, login.UserName);
            if (usrResult is { Success: true, Value: not null })
                user = usrResult.Value;
        }
        else if (!login.Email.IsNullOrWhiteSpace())
        {
            usrResult = await _authDataRepository.GetUserAsync(string.Empty, login.Email, string.Empty);
            if (usrResult is { Success: true, Value: not null })
                user = usrResult.Value;
        }
        else
            return Result<AuthenticationStateModel>.Failed("Username & Email is empty, cannot login!");

        if (user == null || !usrResult.Success)
            return new Result<AuthenticationStateModel>(usrResult);

        if (user.PasswordHash != login.Password) //TODO: Hash password and compare
            return Result<AuthenticationStateModel>.Failed("Login failed, invalid password.");
        var state = new AuthenticationStateModel(user);
        await _authDataRepository.CreateAuthenticationStateAsync(state);
        return Result<AuthenticationStateModel>.Ok(state);
    }

    public async Task<Result> Logout(string state)
    {
        if (state.IsNullOrWhiteSpace())
            return Result.Failed($"Argument {nameof(state)} is empty!");
        /*var stateResult = await _dataProvider.GetAuthenticationStateAsync(state);
        if (!stateResult.Success || stateResult.Value == null)
            return stateResult;
        var authState = stateResult.Value;
        //TODO: Update state to logged out and update the state

        var updatedStateResult = await _dataProvider.UpdateAuthenticationStateAsync(authState);
        if (updatedStateResult.Success) return updatedStateResult;
        _logger.Warning(updatedStateResult.Message);
        return updatedStateResult;*/
        return Result.Failed($"Argument {nameof(state)} is empty!"); // <- TEMP
    }
}