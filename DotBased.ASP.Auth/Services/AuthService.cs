using DotBased.ASP.Auth.Domains;
using DotBased.ASP.Auth.Domains.Auth;
using DotBased.Extensions;
using DotBased.Logging;

namespace DotBased.ASP.Auth.Services;

public class AuthService
{
    public AuthService(AuthDataCache dataCache)
    {
        _dataCache = dataCache;
        _logger = LogService.RegisterLogger(typeof(AuthService));
    }

    private readonly AuthDataCache _dataCache;
    private readonly ILogger _logger;

    public async Task<Result<AuthenticationStateModel>> LoginAsync(LoginModel login)
    {
        if (login.UserName.IsNullOrWhiteSpace())
            return Result<AuthenticationStateModel>.Failed("Username argument is empty!");
        //var userResult = await _dataProvider.GetUserAsync(string.Empty, login.Email, login.UserName);
        //TODO: validate user password and create a session state
        return Result<AuthenticationStateModel>.Failed("");
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