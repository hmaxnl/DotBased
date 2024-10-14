using System.Security.Claims;
using DotBased.ASP.Auth.Scheme;
using DotBased.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using ILogger = DotBased.Logging.ILogger;

namespace DotBased.ASP.Auth;

// RevalidatingServerAuthenticationStateProvider
// AuthenticationStateProvider
// Handles roles
public class BasedServerAuthenticationStateProvider : ServerAuthenticationStateProvider
{
    public BasedServerAuthenticationStateProvider(BasedAuthConfiguration configuration, ISessionStateProvider stateProvider)
    {
        _config = configuration;
        _stateProvider = stateProvider;
        _logger = LogService.RegisterLogger(typeof(BasedServerAuthenticationStateProvider));
    }

    private BasedAuthConfiguration _config;
    private ISessionStateProvider _stateProvider;
    private ILogger _logger;
    private readonly AuthenticationState _loggedInState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Role, "Admin"), new Claim(ClaimTypes.Name, "Anon") }, BasedAuthenticationHandler.AuthenticationScheme)));
    private readonly AuthenticationState _anonState = new AuthenticationState(new ClaimsPrincipal());

    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_loggedInState);
    }
}