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
    AuthorityManager manager) : AuthenticationHandler<AuthorityLoginOptions>(options, logger, encoder)
{
    // Validate credentials
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }
}