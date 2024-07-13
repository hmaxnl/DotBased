using System.Security.Claims;
using DotBased.Logging;
using Microsoft.AspNetCore.Components.Authorization;

namespace DotBased.ASP.Auth;

// RevalidatingServerAuthenticationStateProvider
public class BasedAuthenticationStateProvider : AuthenticationStateProvider
{
    public BasedAuthenticationStateProvider(BasedAuthConfiguration configuration)
    {
        _config = configuration;
        _logger = LogService.RegisterLogger(typeof(BasedAuthenticationStateProvider));
    }

    private BasedAuthConfiguration _config;
    private ILogger _logger;
    private AuthenticationState _anonState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() {new Claim(ClaimTypes.Role, "test")})));
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(_anonState);
    }
}