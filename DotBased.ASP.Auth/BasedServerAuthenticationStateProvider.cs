using System.Security.Claims;
using DotBased.ASP.Auth.Services;
using DotBased.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using ILogger = DotBased.Logging.ILogger;

namespace DotBased.ASP.Auth;

// RevalidatingServerAuthenticationStateProvider
// AuthenticationStateProvider
// Handles roles
public class BasedServerAuthenticationStateProvider : ServerAuthenticationStateProvider
{
    public BasedServerAuthenticationStateProvider(BasedAuthConfiguration configuration, ProtectedLocalStorage localStorage, SecurityService securityService)
    {
        _config = configuration;
        //_stateProvider = stateProvider;
        _localStorage = localStorage;
        _securityService = securityService;
        _logger = LogService.RegisterLogger(typeof(BasedServerAuthenticationStateProvider));
    }

    private BasedAuthConfiguration _config;
    private ISessionStateProvider _stateProvider;
    private ProtectedLocalStorage _localStorage;
    private SecurityService _securityService;
    private ILogger _logger;
    private readonly AuthenticationState _loggedInState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Role, "Admin"),new Claim(ClaimTypes.Role, "nottest"), new Claim(ClaimTypes.Name, "Anon") }, BasedAuthDefaults.AuthenticationScheme)));
    private readonly AuthenticationState _anonState = new AuthenticationState(new ClaimsPrincipal());

    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var sessionIdResult = await _localStorage.GetAsync<string>("dotbased_session");
        if (!sessionIdResult.Success || sessionIdResult.Value == null)
            return _anonState;
        var stateResult = await _securityService.GetAuthenticationFromSession(sessionIdResult.Value);
        return stateResult is { Success: true, Value: not null } ? stateResult.Value : _anonState;
    }
}