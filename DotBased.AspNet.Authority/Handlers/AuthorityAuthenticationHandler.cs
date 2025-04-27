using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace DotBased.AspNet.Authority.Handlers;

/// <summary>
/// Handles authentication for Authority logins.
/// </summary>
public class AuthorityAuthenticationHandler : IAuthenticationSignInHandler
{
    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        throw new NotImplementedException();
    }

    public Task<AuthenticateResult> AuthenticateAsync()
    {
        throw new NotImplementedException();
    }

    public Task ChallengeAsync(AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public Task ForbidAsync(AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public Task SignOutAsync(AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }
}