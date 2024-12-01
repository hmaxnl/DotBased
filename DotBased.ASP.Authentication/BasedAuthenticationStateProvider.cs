using Microsoft.AspNetCore.Components.Authorization;

namespace DotBased.ASP.Authentication;

public class BasedAuthenticationStateProvider : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }
}