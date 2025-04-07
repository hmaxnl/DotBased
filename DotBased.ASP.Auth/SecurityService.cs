using System.Security.Claims;
using DotBased.ASP.Auth.Domains;
using DotBased.ASP.Auth.Domains.Auth;
using DotBased.ASP.Auth.Domains.Identity;
using DotBased.Extensions;
using DotBased.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace DotBased.ASP.Auth;

public class SecurityService
{
    public SecurityService(IAuthDataRepository authDataRepository, AuthDataCache dataCache, ProtectedLocalStorage localStorage)
    {
        _authDataRepository = authDataRepository;
        _dataCache = dataCache;
        _localStorage = localStorage;
        _logger = LogService.RegisterLogger<SecurityService>();
    }

    private readonly IAuthDataRepository _authDataRepository;
    private readonly AuthDataCache _dataCache;
    private readonly ProtectedLocalStorage _localStorage;
    private readonly ILogger _logger;

    public async Task<ResultOld<AuthenticationState>> GetAuthenticationStateFromSessionAsync(string id)
    {
        if (id.IsNullOrEmpty())
            return ResultOld<AuthenticationState>.Failed("No valid id!");
        AuthenticationStateModel? authStateModel = null;
        var stateCache = _dataCache.RequestSessionState(id);
        if (!stateCache.Success || stateCache.Value == null)
        {
            var stateResult = await _authDataRepository.GetAuthenticationStateAsync(id);
            if (stateResult is { Success: true, Value: not null })
            {
                authStateModel = stateResult.Value;
                _dataCache.CacheSessionState(authStateModel);
            }
        }
        else
        {
            if (stateCache.Value.Item2 != null)
                return ResultOld<AuthenticationState>.Ok(stateCache.Value.Item2);
            authStateModel = stateCache.Value.Item1;
        }

        if (authStateModel == null)
            return ResultOld<AuthenticationState>.Failed("Failed to get auth state!");

        var userResult = await _authDataRepository.GetUserAsync(authStateModel.UserId, string.Empty, string.Empty);
        if (userResult is not { Success: true, Value: not null })
            return ResultOld<AuthenticationState>.Failed("Failed to get user from state!");
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Sid, userResult.Value.Id),
            new(ClaimTypes.Name, userResult.Value.Name),
            new(ClaimTypes.NameIdentifier, userResult.Value.UserName),
            new(ClaimTypes.Surname, userResult.Value.FamilyName),
            new(ClaimTypes.Email, userResult.Value.Email)
        };
        claims.AddRange(userResult.Value.Groups.Select(group => new Claim(ClaimTypes.GroupSid, group.Id)));
        claims.AddRange(userResult.Value.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        claims.AddRange(userResult.Value.Groups.Select(g => g.Roles).SelectMany(gRolesList => gRolesList, (_, role) => new Claim(ClaimTypes.Role, role.Name)));
        var claimsIdentity = new ClaimsIdentity(claims, BasedAuthDefaults.AuthenticationScheme);
        var authState = new AuthenticationState(new ClaimsPrincipal(claimsIdentity));
        _dataCache.CacheSessionState(authStateModel, authState);
        return ResultOld<AuthenticationState>.Ok(authState);
    }

    public async Task<ResultOld<AuthenticationStateModel>> LoginAsync(LoginModel login)
    {
        try
        {
            UserModel? user = null;
            ResultOld<UserModel> usrResultOld;
            if (!login.UserName.IsNullOrEmpty())
            {
                usrResultOld = await _authDataRepository.GetUserAsync(string.Empty, string.Empty, login.UserName);
                if (usrResultOld is { Success: true, Value: not null })
                    user = usrResultOld.Value;
            }
            else if (!login.Email.IsNullOrEmpty())
            {
                usrResultOld = await _authDataRepository.GetUserAsync(string.Empty, login.Email, string.Empty);
                if (usrResultOld is { Success: true, Value: not null })
                    user = usrResultOld.Value;
            }
            else
                return ResultOld<AuthenticationStateModel>.Failed("Username & Email is empty, cannot login!");

            if (user == null || !usrResultOld.Success)
                return ResultOld<AuthenticationStateModel>.Failed("No user found!");

            if (user.PasswordHash != login.Password) //TODO: Hash password and compare
                return ResultOld<AuthenticationStateModel>.Failed("Login failed, invalid password.");
            var state = new AuthenticationStateModel(user);
            var authResult = await _authDataRepository.CreateAuthenticationStateAsync(state);
            if (!authResult.Success)
                return ResultOld<AuthenticationStateModel>.Failed("Failed to store session to database!");
            _dataCache.CacheSessionState(state);
            await _localStorage.SetAsync(BasedAuthDefaults.StorageKey, state.Id);
            return ResultOld<AuthenticationStateModel>.Ok(state);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Failed to login!");
            return ResultOld<AuthenticationStateModel>.Failed("Login failed, exception thrown!");
        }
    }

    public async Task<ResultOld> LogoutAsync(string state)
    {
        try
        {
            if (state.IsNullOrEmpty())
                return ResultOld.Failed($"Argument {nameof(state)} is empty!");

            var stateResult = await _authDataRepository.GetAuthenticationStateAsync(state);
            if (!stateResult.Success || stateResult.Value == null)
                return stateResult;
            var authState = stateResult.Value;

            _dataCache.PurgeSessionState(state);
            var updatedStateResult = await _authDataRepository.DeleteAuthenticationStateAsync(authState);
            if (updatedStateResult.Success) return updatedStateResult;
            _logger.Warning(updatedStateResult.Message);
            return updatedStateResult;
        }
        catch (Exception e)
        {
            _logger.Error(e, "Failed to logout!");
            return ResultOld.Failed("Failed to logout, exception thrown!");
        }
    }
}