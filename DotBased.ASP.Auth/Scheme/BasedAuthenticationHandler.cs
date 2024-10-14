using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotBased.ASP.Auth.Scheme;

public class BasedAuthenticationHandler : AuthenticationHandler<BasedAuthenticationHandlerOptions>
{
    public const string AuthenticationScheme = "DotBasedAuthentication";

#pragma warning disable CS0618 // Type or member is obsolete
    public BasedAuthenticationHandler(IOptionsMonitor<BasedAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
#pragma warning restore CS0618 // Type or member is obsolete
    {
        
    }

    public BasedAuthenticationHandler(IOptionsMonitor<BasedAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
        
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        /*var principal = new ClaimsPrincipal();*/
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>() { new Claim(ClaimTypes.Role, "Admin"), new Claim(ClaimTypes.Name, "Anon") }, AuthenticationScheme));
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}