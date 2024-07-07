using DotBased.Logging;
using Microsoft.AspNetCore.Components.Authorization;

namespace DotBased.ASP.Auth;

public class BasedAuthenticationStateProvider : AuthenticationStateProvider
{
    public BasedAuthenticationStateProvider()
    {
        _logger = LogService.RegisterLogger(typeof(BasedAuthenticationStateProvider));
    }

    private ILogger _logger;
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        
        throw new NotImplementedException();
    }
}