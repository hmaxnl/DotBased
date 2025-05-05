using System.Security.Claims;
using System.Text.Encodings.Web;
using DotBased.AspNet.Authority.Managers;
using DotBased.AspNet.Authority.Models.Options.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotBased.AspNet.Authority.Handlers;

/// <summary>
/// Handles authentication for Authority logins.
/// </summary>
public class AuthorityLoginAuthenticationHandler(IOptionsMonitor<AuthorityLoginOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AuthorityManager manager) : SignInAuthenticationHandler<AuthorityLoginOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }

    protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }
}