using System.Security.Claims;
using DotBased.Logging;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
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
        _localStorage = localStorage;
        _securityService = securityService;
        _logger = LogService.RegisterLogger<BasedServerAuthenticationStateProvider>();
    }

    private BasedAuthConfiguration _config;
    private readonly ProtectedLocalStorage _localStorage;
    private readonly SecurityService _securityService;
    private readonly ILogger _logger;
    private readonly AuthenticationState _anonState = new(new ClaimsPrincipal());

    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.Debug("Getting authentication state...");
        var sessionIdResult = await _localStorage.GetAsync<string>(BasedAuthDefaults.StorageKey);
        if (!sessionIdResult.Success || sessionIdResult.Value == null)
            return _anonState;
        _logger.Debug("Found state [{State}], getting session from {Service}", sessionIdResult.Value, nameof(SecurityService));
        var stateResult = await _securityService.GetAuthenticationStateFromSessionAsync(sessionIdResult.Value);
        return stateResult is { Success: true, Value: not null } ? stateResult.Value : _anonState;
    }
}